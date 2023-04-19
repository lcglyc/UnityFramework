/*自动生成的代码，千万不要修改,如需添加，请联系 Arvin 2019.04.03*/
using System;
using LitJson;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace MonogolyConfig{

public class UpgradeData
{
	private int id; // ID
	public int ID {get{return id;} set{id=value;}}

	private int ballid; // BallID
	public int BallID {get{return ballid;} set{ballid=value;}}

	private int upgradelv; // UpgradeLv
	public int UpgradeLv {get{return upgradelv;} set{upgradelv=value;}}

	private int upgradestage; // UpgradeStage
	public int UpgradeStage {get{return upgradestage;} set{upgradestage=value;}}

	private int upgrademode; // UpgradeMode
	public int UpgradeMode {get{return upgrademode;} set{upgrademode=value;}}

	private int upgradetype; // UpgradeType
	public int UpgradeType {get{return upgradetype;} set{upgradetype=value;}}

	private int attrtype; // AttrType
	public int AttrType {get{return attrtype;} set{attrtype=value;}}

	private float attrvalue; // AttrValue
	public float AttrValue {get{return attrvalue;} set{attrvalue=value;}}

	private int cost; // Cost
	public int Cost {get{return cost;} set{cost=value;}}

}
// End of Auto Generated Code
public class UpgradeDataManager
{
private static UpgradeDataManager instance;
private static object _lock = new object();
public static UpgradeDataManager GetInstance(){

 if (instance == null){lock (_lock){if (instance == null){
instance = new UpgradeDataManager();
}}}return instance;}
private UpgradeDataManager(){
}
	private Dictionary<int,UpgradeData>dict=new Dictionary<int,UpgradeData>(); 
	   public Dictionary<int, UpgradeData> GetConfigDic(){
return dict;}

	public  UpgradeData GetUpgradeDataInfo(int key)
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
 UpgradeData upgradedata = new UpgradeData();  upgradedata.ID=GetInt(item["ID"].ToString());
 upgradedata.BallID=GetInt(item["BallID"].ToString());
 upgradedata.UpgradeLv=GetInt(item["UpgradeLv"].ToString());
 upgradedata.UpgradeStage=GetInt(item["UpgradeStage"].ToString());
 upgradedata.UpgradeMode=GetInt(item["UpgradeMode"].ToString());
 upgradedata.UpgradeType=GetInt(item["UpgradeType"].ToString());
 upgradedata.AttrType=GetInt(item["AttrType"].ToString());
 upgradedata.AttrValue = GetFloat(item["AttrValue"].ToString());
 upgradedata.Cost=GetInt(item["Cost"].ToString());
if (dict.ContainsKey(upgradedata.ID) == false){
 dict.Add(upgradedata.ID, upgradedata);
}
}
Debug.Log( "读取表 UpgradeData Manager结束,共:" + dict.Count.ToString());}
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
