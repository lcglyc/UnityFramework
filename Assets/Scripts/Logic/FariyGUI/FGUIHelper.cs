 
using FairyGUI;
using ECSModel;
using System;
using System.Diagnostics;
using UnityEditor;

public class FGUIHelper
{
    public static GButton GetButton(string name, FUI panel)
    {
        FUI controls = panel.Get(name);
        if (controls == null)
        {
            Log.Error("GetBtn Error:" + name);
            return null;
        }

        return controls.GObject.asButton;
    }

    // 回调无参数
    public static GButton GetButton( string btnName,FUI panel , EventCallback0 callback)
    {
        GButton btn = GetButton(btnName, panel);
        btn.onClick.Add(callback);
        
        return btn;
    }

    public static GButton GetTouchMoveBtn( string btnName, FUI panel,EventCallback0 callback )
    {
        GButton btn = GetButton(btnName, panel);
        btn.onTouchMove.Add(callback);
        return btn;
    }

    public static GButton GetTouchBeginBtn(string btnName, FUI panel, EventCallback0 callback)
    {
        GButton btn = GetButton(btnName, panel);
        btn.onTouchBegin.Add(callback);
        return btn;
    }

    public static GButton GetTouchEndBtn(string btnName, FUI panel, EventCallback0 callback)
    {
        GButton btn = GetButton(btnName, panel);
        btn.onTouchEnd.Add(callback);
        return btn;
    }


    // 回调有参数
    public static GButton GetButton( string btnName,FUI panel,EventCallback1 callback)
    {
        GButton btn = GetButton(btnName, panel);
        btn.onClick.Add(callback);
        return btn;
    }

    public static GSlider GetSlider(string btnName, FUI panel, EventCallback1 callback)
    {
        GSlider slider = panel.Get(btnName).GObject.asSlider;
        slider.onChanged.Add(callback);
        return slider;
    }

    public static Transition GetTransition( string ctrlName, FUI panel)
    {
        Transition ctrl = panel.GetTransition(ctrlName);
        if(ctrl == null)
        {
            Log.Error("Get Transition error:" + ctrlName);
            return null;
        }

        return ctrl;
    }

    public static void PlayerTransition( Transition ctrl )
    {
        float time = ctrl.GetLabelTime("EndEvent");
        ctrl.Play(1, 0.0f, 0.0f, time, () =>
        {
            Log.Debug("stop");
        });
    }

    public static GTextField GetTextField(string name, FUI panel)
    {
        FUI controls = panel.Get(name);
        if (controls == null)
        {
            Log.Error("GetBtn Error:" + name);
            return null;
        }
        return controls.GObject.asTextField;
    }

    public static GProgressBar GetProgressBar(string name, FUI panel)
    {
        FUI controls = panel.Get(name);
        if (controls == null)
        {
            Log.Error("GetBtn Error:" + name);
            return null;
        }

        return controls.GObject.asProgress;
    }

    public static  void  SwipeGesture( GObject holder, EventCallback1 onMove, EventCallback1 onEnd )
    {
        SwipeGesture gesture1 = new SwipeGesture(holder);
        gesture1.onMove.Add(onMove);
        gesture1.onEnd.Add(onEnd);
    }

    public static Controller GetController(  string name, FUI panel )
    {
        return panel.GetController(name);
    }
}
