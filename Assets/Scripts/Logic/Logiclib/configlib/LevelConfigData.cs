/*自动生成的代码，千万不要修改,如需添加，请联系 Arvin 2019.04.03*/
using System;
using LitJson;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace MonogolyConfig{

public class LevelConfigData
{
	private int id; // ID
	public int ID {get{return id;} set{id=value;}}

	private int levelid; // LevelID
	public int LevelID {get{return levelid;} set{levelid=value;}}

	private int wave; // Wave
	public int Wave {get{return wave;} set{wave=value;}}

	private int wavetype; // WaveType
	public int WaveType {get{return wavetype;} set{wavetype=value;}}

	private int bricktype; // BrickType
	public int BrickType {get{return bricktype;} set{bricktype=value;}}

	private string brickvalue; // BrickValue
	public string BrickValue {get{return brickvalue;} set{brickvalue=value;}}

	private int hp; // HP
	public int HP {get{return hp;} set{hp=value;}}

	private int tileposx; // TilePosX
	public int TilePosX {get{return tileposx;} set{tileposx=value;}}

	private int tileposy; // TilePosY
	public int TilePosY {get{return tileposy;} set{tileposy=value;}}

	private float unityposx; // UnityPosX
	public float UnityPosX {get{return unityposx;} set{unityposx=value;}}

	private float unityposy; // UnityPosY
	public float UnityPosY {get{return unityposy;} set{unityposy=value;}}

	private string prefab; // Prefab
	public string Prefab {get{return prefab;} set{prefab=value;}}

	private string res; // Res
	public string Res {get{return res;} set{res=value;}}

	private string brokenres1; // BrokenRes1
	public string BrokenRes1 {get{return brokenres1;} set{brokenres1=value;}}

	private string brokenres2; // BrokenRes2
	public string BrokenRes2 {get{return brokenres2;} set{brokenres2=value;}}

	private string deathanim; // DeathAnim
	public string DeathAnim {get{return deathanim;} set{deathanim=value;}}

	private int reward; // Reward
	public int Reward {get{return reward;} set{reward=value;}}

}
// End of Auto Generated Code
public class LevelConfigDataManager
{
private static LevelConfigDataManager instance;
private static object _lock = new object();
public static LevelConfigDataManager GetInstance(){

 if (instance == null){lock (_lock){if (instance == null){
instance = new LevelConfigDataManager();
}}}return instance;}
private LevelConfigDataManager(){
}
	private Dictionary<int,LevelConfigData>dict=new Dictionary<int,LevelConfigData>(); 
	   public Dictionary<int, LevelConfigData> GetConfigDic(){
return dict;}

	public  LevelConfigData GetLevelConfigDataInfo(int key)
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
 LevelConfigData levelconfigdata = new LevelConfigData();  levelconfigdata.ID=GetInt(item["ID"].ToString());
 levelconfigdata.LevelID=GetInt(item["LevelID"].ToString());
 levelconfigdata.Wave=GetInt(item["Wave"].ToString());
 levelconfigdata.WaveType=GetInt(item["WaveType"].ToString());
 levelconfigdata.BrickType=GetInt(item["BrickType"].ToString());
levelconfigdata.BrickValue=item["BrickValue"].ToString();
 levelconfigdata.HP=GetInt(item["HP"].ToString());
 levelconfigdata.TilePosX=GetInt(item["TilePosX"].ToString());
 levelconfigdata.TilePosY=GetInt(item["TilePosY"].ToString());
 levelconfigdata.UnityPosX = GetFloat(item["UnityPosX"].ToString());
 levelconfigdata.UnityPosY = GetFloat(item["UnityPosY"].ToString());
levelconfigdata.Prefab=item["Prefab"].ToString();
levelconfigdata.Res=item["Res"].ToString();
levelconfigdata.BrokenRes1=item["BrokenRes1"].ToString();
levelconfigdata.BrokenRes2=item["BrokenRes2"].ToString();
levelconfigdata.DeathAnim=item["DeathAnim"].ToString();
 levelconfigdata.Reward=GetInt(item["Reward"].ToString());
if (dict.ContainsKey(levelconfigdata.ID) == false){
 dict.Add(levelconfigdata.ID, levelconfigdata);
}
}
Debug.Log( "读取表 LevelConfigData Manager结束,共:" + dict.Count.ToString());}
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
