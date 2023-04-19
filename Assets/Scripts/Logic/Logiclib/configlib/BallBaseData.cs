/*自动生成的代码，千万不要修改,如需添加，请联系 Arvin 2019.04.03*/
using System;
using LitJson;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace MonogolyConfig{

[Serializable]
public class BallBaseData
{
	private int id; // ID
	public int ID {get{return id;} set{id=value;}}

	private string ballname; // BallName
	public string BallName {get{return ballname;} set{ballname=value;}}

	private string ability; // Ability
	public string Ability {get{return ability;} set{ability=value;}}

	private string ballres; // BallRes
	public string BallRes {get{return ballres;} set{ballres=value;}}

	private int ballnum; // BallNum
	public int BallNum {get{return ballnum;} set{ballnum=value;}}

	private float defalutscale; // DefalutScale
	public float DefalutScale {get{return defalutscale;} set{defalutscale=value;}}

	private int baseatk; // BaseAtk
	public int BaseAtk {get{return baseatk;} set{baseatk=value;}}

	private float basespd; // BaseSpd
	public float BaseSpd {get{return basespd;} set{basespd=value;}}

	private float bullettimeparam; // BulletTimeParam
	public float BulletTimeParam {get{return bullettimeparam;} set{bullettimeparam=value;}}

	private float frozenframeparam; // FrozenFrameParam
	public float FrozenFrameParam {get{return frozenframeparam;} set{frozenframeparam=value;}}

	private int unlocktype; // UnlockType
	public int UnlockType {get{return unlocktype;} set{unlocktype=value;}}

	private int unlockvalue; // UnlockValue
	public int UnlockValue {get{return unlockvalue;} set{unlockvalue=value;}}

}
// End of Auto Generated Code
public class BallBaseDataManager
{
private static BallBaseDataManager instance;
private static object _lock = new object();
public static BallBaseDataManager GetInstance(){

 if (instance == null){lock (_lock){if (instance == null){
instance = new BallBaseDataManager();
}}}return instance;}
private BallBaseDataManager(){
}
	private Dictionary<int,BallBaseData>dict=new Dictionary<int,BallBaseData>(); 
	   public Dictionary<int, BallBaseData> GetConfigDic(){
return dict;}

	public  BallBaseData GetBallBaseDataInfo(int key)
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
 BallBaseData ballbasedata = new BallBaseData();  ballbasedata.ID=GetInt(item["ID"].ToString());
ballbasedata.BallName=item["BallName"].ToString();
ballbasedata.Ability=item["Ability"].ToString();
ballbasedata.BallRes=item["BallRes"].ToString();
 ballbasedata.BallNum=GetInt(item["BallNum"].ToString());
 ballbasedata.DefalutScale = GetFloat(item["DefalutScale"].ToString());
 ballbasedata.BaseAtk=GetInt(item["BaseAtk"].ToString());
 ballbasedata.BaseSpd = GetFloat(item["BaseSpd"].ToString());
 ballbasedata.BulletTimeParam = GetFloat(item["BulletTimeParam"].ToString());
 ballbasedata.FrozenFrameParam = GetFloat(item["FrozenFrameParam"].ToString());
 ballbasedata.UnlockType=GetInt(item["UnlockType"].ToString());
 ballbasedata.UnlockValue=GetInt(item["UnlockValue"].ToString());
if (dict.ContainsKey(ballbasedata.ID) == false){
 dict.Add(ballbasedata.ID, ballbasedata);
}
}
Debug.Log( "读取表 BallBaseData Manager结束,共:" + dict.Count.ToString());}
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
