/*自动生成的代码，千万不要修改,如需添加，请联系 Arvin 2019.04.03*/
using System;
using LitJson;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace MonogolyConfig{

public class BoardBaseData
{
	private int id; // ID
	public int ID {get{return id;} set{id=value;}}

	private string boardname; // BoardName
	public string BoardName {get{return boardname;} set{boardname=value;}}

	private string ability; // Ability
	public string Ability {get{return ability;} set{ability=value;}}

	private string desc; // Desc
	public string Desc {get{return desc;} set{desc=value;}}

	private string boardres; // BoardRes
	public string BoardRes {get{return boardres;} set{boardres=value;}}

	private float defalutscale; // DefalutScale
	public float DefalutScale {get{return defalutscale;} set{defalutscale=value;}}

	private int weapontype; // WeaponType
	public int WeaponType {get{return weapontype;} set{weapontype=value;}}

	private int baseatk; // BaseAtk
	public int BaseAtk {get{return baseatk;} set{baseatk=value;}}

	private int weaponvalue1; // WeaponValue1
	public int WeaponValue1 {get{return weaponvalue1;} set{weaponvalue1=value;}}

	private int weaponvalue2; // WeaponValue2
	public int WeaponValue2 {get{return weaponvalue2;} set{weaponvalue2=value;}}

	private float basespd; // BaseSpd
	public float BaseSpd {get{return basespd;} set{basespd=value;}}

	private int unlocktype; // UnlockType
	public int UnlockType {get{return unlocktype;} set{unlocktype=value;}}

	private int unlockvalue; // UnlockValue
	public int UnlockValue {get{return unlockvalue;} set{unlockvalue=value;}}

}
// End of Auto Generated Code
public class BoardBaseDataManager
{
private static BoardBaseDataManager instance;
private static object _lock = new object();
public static BoardBaseDataManager GetInstance(){

 if (instance == null){lock (_lock){if (instance == null){
instance = new BoardBaseDataManager();
}}}return instance;}
private BoardBaseDataManager(){
}
	private Dictionary<int,BoardBaseData>dict=new Dictionary<int,BoardBaseData>(); 
	   public Dictionary<int, BoardBaseData> GetConfigDic(){
return dict;}

	public  BoardBaseData GetBoardBaseDataInfo(int key)
{
 if(dict.ContainsKey(key))
{
return dict[key];
}
 	Debug.LogError("not has this key");
return null;
}
public void ReadData( string configdata){
LitJson.JsonData array = JsonMapper.ToObject(configdata);
 foreach (JsonData item in array){
 BoardBaseData boardbasedata = new BoardBaseData();  boardbasedata.ID=GetInt(item["ID"].ToString());
boardbasedata.BoardName=item["BoardName"].ToString();
boardbasedata.Ability=item["Ability"].ToString();
boardbasedata.Desc=item["Desc"].ToString();
boardbasedata.BoardRes=item["BoardRes"].ToString();
 boardbasedata.DefalutScale = GetFloat(item["DefalutScale"].ToString());
 boardbasedata.WeaponType=GetInt(item["WeaponType"].ToString());
 boardbasedata.BaseAtk=GetInt(item["BaseAtk"].ToString());
 boardbasedata.WeaponValue1=GetInt(item["WeaponValue1"].ToString());
 boardbasedata.WeaponValue2=GetInt(item["WeaponValue2"].ToString());
 boardbasedata.BaseSpd = GetFloat(item["BaseSpd"].ToString());
 boardbasedata.UnlockType=GetInt(item["UnlockType"].ToString());
 boardbasedata.UnlockValue=GetInt(item["UnlockValue"].ToString());
if (dict.ContainsKey(boardbasedata.ID) == false){
 dict.Add(boardbasedata.ID, boardbasedata);
}
}
Debug.Log( "读取表 BoardBaseData Manager结束,共:" + dict.Count.ToString());}
private Vector3 GetVector3(string key)
{
Vector3 temp = Vector3.zero;
 key = key.Replace("(", "").Replace(")", "");string[] keys = key.Split(',');
  if (keys.Length != 3)
{
 Debug.LogError("string 转 vector3 出错：" + key);return temp;
}
temp.x = float.Parse(keys[0]);
temp.y = float.Parse(keys[1]);
temp.z = float.Parse(keys[2]);
return temp;}
	private int GetInt(string key){
int value = -1;if (string.IsNullOrEmpty(key)) return value;if (int.TryParse(key, out value)){
return value;}
 if (key == "") return value;
 Debug.LogError("转换 int 数值出错：" + key); return value;}
	 private bool GetBool(string key){
bool value = false;if (bool.TryParse(key, out value)){
return value;}
 Debug.LogError("转换 bool 数值出错：" + key);return value;}
	 int[] GetIntArray(string key){
string[] values = key.Split(',');
int[] intValue = new int[values.Length];
for (int i = 0; i < values.Length; i++){
intValue[i] = int.Parse(values[i]);}
return intValue;}

	 float[] GetFloatArray(string key){
string[] values = key.Split(',');
float[] intValue = new float[values.Length];
for (int i = 0; i < values.Length; i++){
intValue[i] = float.Parse(values[i]);}
return intValue;}
	 private float GetFloat(string key){
 float value = 0.0f;if (string.IsNullOrEmpty(key)) return value;if (float.TryParse(key, out value)){
return value;}
 Debug.LogError("转换 float 数值出错：" + key);return value;}
}
}
