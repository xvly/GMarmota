#target photoshop
app.bringToFront();

var sizeList =
[
  {"name": "iTunesArtwork",        "size":1024},
  {"name": "Icon",                      "size":57},
  {"name": "Icon@2x",                "size":114},
  {"name": "Icon-72",                  "size":72},
  {"name": "Icon-76",                  "size":76},
  {"name": "Icon-120",                "size":120},
  {"name": "Icon-144",                  "size":144},
  {"name": "Icon-152",                "size":152},
  {"name": "Icon-167",                "size":167},
  {"name": "Icon-180",                "size":180}
];

var fileRef = File.openDialog ("请选择一个文件", "*.png", false);

// 选择输出目录
//var destFolder = Folder.selectDialog( "请选择一个输出的文件夹："); 

// 输出到图片所在目录
var destFolder = fileRef.parent; 

//打开文件
var activeDocument = app.open(fileRef);

//运行批处理尺寸
runNow() ;

function runNow()
{
     if(activeDocument.height != activeDocument.width)
     {
         alert("当前文件宽高尺寸不一致，脚本已中止。");
         return;
     }
	 
	//以PNG格式保存，带压缩
	var pngSaveOptions = new ExportOptionsSaveForWeb();
	pngSaveOptions.format = SaveDocumentType.PNG;
	pngSaveOptions.transparency = true;
	pngSaveOptions.PNG8 = false;
	pngSaveOptions.interlaced = false;
	//pngSaveOptions.quality = 30;
	 
     for(var i = 0; i < sizeList.length; i ++)
     {
         //重置图像尺寸
         activeDocument.resizeImage(UnitValue(sizeList[i].size,"px"),UnitValue(sizeList[i].size,"px"),null,ResampleMethod.BICUBIC);

         var destFileName = destFolder + "/" + sizeList[i].name +".png";

         //保存的文件
         var saveFile = new File(destFileName);

         //如果文件已经存在就先删除它
         if (saveFile.exists)
         {
               saveFile.remove();
         }

         activeDocument.exportDocument(saveFile, ExportType.SAVEFORWEB, pngSaveOptions);
         activeDocument.activeHistoryState = activeDocument.historyStates[0];//还原到打开状态
     }
     activeDocument.close(SaveOptions.DONOTSAVECHANGES);//原始被打开的文件不保存，关闭源文件
}