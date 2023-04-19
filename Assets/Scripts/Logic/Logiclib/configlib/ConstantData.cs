/*自动生成的代码，千万不要修改,如需添加，请联系 Arvin 2019.04.03*/
using System;
using LitJson;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace MonogolyConfig{

public class ConstantData
{
	private string name; // Name
	public string Name {get{return name;} set{name=value;}}

	private string[] valuekey; // ValueKey
	public string[] ValueKey {get{return valuekey;} set{valuekey=value;}}

}
// End of Auto Generated Code
public class ConstantDataManager
{
private static ConstantDataManager instance;
private static object _lock = new object();
public static ConstantDataManager GetInstance(){

 if (instance == null){lock (_lock){if (instance == null){
instance = new ConstantDataManager();
}}}return instance;}
private ConstantDataManager(){
}
	private Dictionary<string,ConstantData>dict=new Dictionary<string,ConstantData>(); 
	   public Dictionary<string, ConstantData> GetConfigDic(){
return dict;}

	public  ConstantData GetConstantDataInfo(string key)
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
 ConstantData constantdata = new ConstantData(); constantdata.Name=item["Name"].ToString();
constantdata.ValueKey= item["ValueKey"].ToString().Split(',');
if (dict.ContainsKey(constantdata.Name) == false){
 dict.Add(constantdata.Name, constantdata);
}
}
Debug.Log( "读取表 ConstantData Manager结束,共:" + dict.Count.ToString());}
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
