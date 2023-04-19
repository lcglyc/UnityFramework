/*自动生成的代码，千万不要修改,如需添加，请联系 Arvin 2019.04.03*/
using System;
using LitJson;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace MonogolyConfig{

public class LevelData
{
	private int id; // ID
	public int ID {get{return id;} set{id=value;}}

	private int leveltype; // LevelType
	public int LevelType {get{return leveltype;} set{leveltype=value;}}

	private float cellsizex; // CellSizeX
	public float CellSizeX {get{return cellsizex;} set{cellsizex=value;}}

	private float cellsizey; // CellSizeY
	public float CellSizeY {get{return cellsizey;} set{cellsizey=value;}}

	private float tileanchorx; // TileAnchorX
	public float TileAnchorX {get{return tileanchorx;} set{tileanchorx=value;}}

	private float tileanchory; // TileAnchorY
	public float TileAnchorY {get{return tileanchory;} set{tileanchory=value;}}

	private int reward; // Reward
	public int Reward {get{return reward;} set{reward=value;}}

	private float gridoffsetx; // GridOffsetX
	public float GridOffsetX {get{return gridoffsetx;} set{gridoffsetx=value;}}

	private string ballsizeparam; // BallSizeParam
	public string BallSizeParam {get{return ballsizeparam;} set{ballsizeparam=value;}}

	private string ballspdparam; // BallSpdParam
	public string BallSpdParam {get{return ballspdparam;} set{ballspdparam=value;}}

	private string boardlengthwparam; // BoardLengthwParam
	public string BoardLengthwParam {get{return boardlengthwparam;} set{boardlengthwparam=value;}}

	private string cameraeffect; // CameraEffect
	public string CameraEffect {get{return cameraeffect;} set{cameraeffect=value;}}

}
// End of Auto Generated Code
public class LevelDataManager
{
private static LevelDataManager instance;
private static object _lock = new object();
public static LevelDataManager GetInstance(){

 if (instance == null){lock (_lock){if (instance == null){
instance = new LevelDataManager();
}}}return instance;}
private LevelDataManager(){
}
	private Dictionary<int,LevelData>dict=new Dictionary<int,LevelData>(); 
	   public Dictionary<int, LevelData> GetConfigDic(){
return dict;}

	public  LevelData GetLevelDataInfo(int key)
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
 LevelData leveldata = new LevelData();  leveldata.ID=GetInt(item["ID"].ToString());
 leveldata.LevelType=GetInt(item["LevelType"].ToString());
 leveldata.CellSizeX = GetFloat(item["CellSizeX"].ToString());
 leveldata.CellSizeY = GetFloat(item["CellSizeY"].ToString());
 leveldata.TileAnchorX = GetFloat(item["TileAnchorX"].ToString());
 leveldata.TileAnchorY = GetFloat(item["TileAnchorY"].ToString());
 leveldata.Reward=GetInt(item["Reward"].ToString());
 leveldata.GridOffsetX = GetFloat(item["GridOffsetX"].ToString());
leveldata.BallSizeParam=item["BallSizeParam"].ToString();
leveldata.BallSpdParam=item["BallSpdParam"].ToString();
leveldata.BoardLengthwParam=item["BoardLengthwParam"].ToString();
leveldata.CameraEffect=item["CameraEffect"].ToString();
if (dict.ContainsKey(leveldata.ID) == false){
 dict.Add(leveldata.ID, leveldata);
}
}
Debug.Log( "读取表 LevelData Manager结束,共:" + dict.Count.ToString());}
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
