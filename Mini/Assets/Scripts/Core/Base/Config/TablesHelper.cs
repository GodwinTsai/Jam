// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using System.IO;
using SimpleJSON;
using UnityEngine;

public class TablesHelper : SingletonClassAuto<TablesHelper>
{
	private string _dir;
	public void InitConfig()
	{
		// // 一行代码可以加载所有配置。 cfg.Tables 包含所有表的一个实例字段。
		// var tables = new cfg.Tables(LoadJson);
		// // 访问一个单例表
		// // MTDebug.Log(tables.TbItem.Name);
		// // 访问普通的 key-value 表'
		// MTDebug.Log("1111111111111111111111111111");
		// MTDebug.Log(tables.TbItem.Get(10008).Name);
		// MTDebug.Log("2222222222222222222222222222222");
		//
		// foreach (var item in tables.TbItem.DataList)
		// {
		// 	MTDebug.Log(item.Name);
		// }
		//
		// // 支持 operator []用法
		// MTDebug.Log(tables.TbItem[10007].Desc);
		//
		// MTDebug.Log($"x1:{tables.TbCommon.X1}, x2:{tables.TbCommon.X2}");
	}

	// private static JSONNode LoadJson(string file)
	// {
	// 	var path = GetConfigPath(file);
	// 	return JSON.Parse(File.ReadAllText(path, System.Text.Encoding.UTF8));
	// }
	//
	// private static string GetConfigPath(string file)
	// {
	// 	return Path.Combine(Application.streamingAssetsPath, "ConfigData", "Json", $"{file}.json");
	// }

}