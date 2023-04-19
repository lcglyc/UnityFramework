/*自动生成的代码，千万不要修改,如需添加，请联系 Arvin 2019.04.03*/
using System;
using LitJson;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace MonogolyConfig{

public class Language
{
	private string key; // Key
	public string Key {get{return key;} set{key=value;}}

	private string cn; // CN
	public string CN {get{return cn;} set{cn=value;}}

	private string en; // EN
	public string EN {get{return en;} set{en=value;}}

}
// End of Auto Generated Code
public class LanguageManager
{
private static LanguageManager instance;
private static object _lock = new object();
public static LanguageManager GetInstance(){

 if (instance == null){lock (_lock){if (instance == null){
instance = new LanguageManager();
}}}return instance;}
private LanguageManager(){
}
	private Dictionary<string,Language>dict=new Dictionary<string,Language>(); 
	   public Dictionary<string, Language> GetConfigDic(){
return dict;}

	public  Language GetLanguageInfo(string key)
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
 Language language = new Language(); language.Key=item["Key"].ToString();
language.CN=item["CN"].ToString();
language.EN=item["EN"].ToString();
if (dict.ContainsKey(language.Key) == false){
 dict.Add(language.Key, language);
}
}
Debug.Log( "读取表 Language Manager结束,共:" + dict.Count.ToString());}
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
