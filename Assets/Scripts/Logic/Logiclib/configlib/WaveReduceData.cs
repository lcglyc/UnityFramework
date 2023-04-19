/*自动生成的代码，千万不要修改,如需添加，请联系 Arvin 2019.04.03*/
using System;
using LitJson;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace MonogolyConfig{

public class WaveReduceData
{
	private int id; // ID
	public int ID {get{return id;} set{id=value;}}

	private string ballsizeparam; // BallSizeParam
	public string BallSizeParam {get{return ballsizeparam;} set{ballsizeparam=value;}}

	private string ballspdparam; // BallSpdParam
	public string BallSpdParam {get{return ballspdparam;} set{ballspdparam=value;}}

	private string boardlengthwparam; // BoardLengthwParam
	public string BoardLengthwParam {get{return boardlengthwparam;} set{boardlengthwparam=value;}}

}
// End of Auto Generated Code
public class WaveReduceDataManager
{
private static WaveReduceDataManager instance;
private static object _lock = new object();
public static WaveReduceDataManager GetInstance(){

 if (instance == null){lock (_lock){if (instance == null){
instance = new WaveReduceDataManager();
}}}return instance;}
private WaveReduceDataManager(){
}
	private Dictionary<int,WaveReduceData>dict=new Dictionary<int,WaveReduceData>(); 
	   public Dictionary<int, WaveReduceData> GetConfigDic(){
return dict;}

	public  WaveReduceData GetWaveReduceDataInfo(int key)
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
 WaveReduceData wavereducedata = new WaveReduceData();  wavereducedata.ID=GetInt(item["ID"].ToString());
wavereducedata.BallSizeParam=item["BallSizeParam"].ToString();
wavereducedata.BallSpdParam=item["BallSpdParam"].ToString();
wavereducedata.BoardLengthwParam=item["BoardLengthwParam"].ToString();
if (dict.ContainsKey(wavereducedata.ID) == false){
 dict.Add(wavereducedata.ID, wavereducedata);
}
}
Debug.Log( "读取表 WaveReduceData Manager结束,共:" + dict.Count.ToString());}
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
int value = -1;if (int.TryParse(key, out value)){
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
 float value = 0.0f;if (float.TryParse(key, out value)){
return value;}
 Debug.LogError("转换 float 数值出错：" + key);return value;}
}
}
