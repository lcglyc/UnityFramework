/*自动生成的代码，千万不要修改,如需添加，请联系 Arvin 2019.04.03*/
using System;
using LitJson;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace MonogolyConfig{

public class BoardUpgradeData
{
	private int id; // ID
	public int ID {get{return id;} set{id=value;}}

	private int boardid; // BoardID
	public int BoardID {get{return boardid;} set{boardid=value;}}

	private int upgrademode; // UpgradeMode
	public int UpgradeMode {get{return upgrademode;} set{upgrademode=value;}}

	private int upgradelv; // UpgradeLv
	public int UpgradeLv {get{return upgradelv;} set{upgradelv=value;}}

	private int attrtype1; // AttrType1
	public int AttrType1 {get{return attrtype1;} set{attrtype1=value;}}

	private float attrvalue1; // AttrValue1
	public float AttrValue1 {get{return attrvalue1;} set{attrvalue1=value;}}

	private int attrtype2; // AttrType2
	public int AttrType2 {get{return attrtype2;} set{attrtype2=value;}}

	private float attrvalue2; // AttrValue2
	public float AttrValue2 {get{return attrvalue2;} set{attrvalue2=value;}}

	private int cost; // Cost
	public int Cost {get{return cost;} set{cost=value;}}

}
// End of Auto Generated Code
public class BoardUpgradeDataManager
{
private static BoardUpgradeDataManager instance;
private static object _lock = new object();
public static BoardUpgradeDataManager GetInstance(){

 if (instance == null){lock (_lock){if (instance == null){
instance = new BoardUpgradeDataManager();
}}}return instance;}
private BoardUpgradeDataManager(){
}
	private Dictionary<int,BoardUpgradeData>dict=new Dictionary<int,BoardUpgradeData>(); 
	   public Dictionary<int, BoardUpgradeData> GetConfigDic(){
return dict;}

	public  BoardUpgradeData GetBoardUpgradeDataInfo(int key)
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
 BoardUpgradeData boardupgradedata = new BoardUpgradeData();  boardupgradedata.ID=GetInt(item["ID"].ToString());
 boardupgradedata.BoardID=GetInt(item["BoardID"].ToString());
 boardupgradedata.UpgradeMode=GetInt(item["UpgradeMode"].ToString());
 boardupgradedata.UpgradeLv=GetInt(item["UpgradeLv"].ToString());
 boardupgradedata.AttrType1=GetInt(item["AttrType1"].ToString());
 boardupgradedata.AttrValue1 = GetFloat(item["AttrValue1"].ToString());
 boardupgradedata.AttrType2=GetInt(item["AttrType2"].ToString());
 boardupgradedata.AttrValue2 = GetFloat(item["AttrValue2"].ToString());
 boardupgradedata.Cost=GetInt(item["Cost"].ToString());
if (dict.ContainsKey(boardupgradedata.ID) == false){
 dict.Add(boardupgradedata.ID, boardupgradedata);
}
}
Debug.Log( "读取表 BoardUpgradeData Manager结束,共:" + dict.Count.ToString());}
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
