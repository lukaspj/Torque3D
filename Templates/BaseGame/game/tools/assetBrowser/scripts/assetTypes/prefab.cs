function AssetBrowser::createPrefab(%this)
{
   %moduleName = AssetBrowser.newAssetSettings.moduleName;
   %modulePath = "data/" @ %moduleName;
      
   %assetName = AssetBrowser.newAssetSettings.assetName;  
   
   %assetPath = AssetBrowser.dirHandler.currentAddress @ "/";    
   
   %tamlpath = %assetPath @ %assetName @ ".asset.taml";
   %prefabFilePath = %assetPath @ %assetName @ ".prefab";
   
   EWorldEditor.makeSelectionPrefab( %prefabFilePath );    
   
   EditorTree.buildVisibleTree( true ); 
}

function AssetBrowser::buildPrefabPreview(%this, %assetDef, %previewData)
{
   %fullPath = %assetDef.dirPath @ "/" @ %assetDef.assetName;
   %previewData.assetName = %assetDef.assetName;
   %previewData.assetPath = %fullPath;
   
   %previewData.previewImage = "tools/assetBrowser/art/genericAssetIcon";
   
   //%previewData.assetFriendlyName = %assetDef.assetName;
   %previewData.assetDesc = %assetDef.description;
   %previewData.tooltip = %fullPath;
   //%previewData.doubleClickCommand = "AssetBrowser.schedule(10, \"navigateTo\",\""@ %assetDef.dirPath @ "/" @ %assetDef.assetName @"\");";//browseTo %assetDef.dirPath / %assetDef.assetName
   %previewData.doubleClickCommand = "AssetBrowser.autoImportFile(\"" @ %fullPath @ "\");";
}

function AssetBrowser::onPrefabEditorDropped(%this, %assetDef, %position)
{
   //echo("DROPPED A SHAPE ON THE EDITOR WINDOW!"); 

   %targetPosition = EWorldEditor.unproject(%position SPC 1000);
   %camPos = LocalClientConnection.camera.getPosition();
   %rayResult = containerRayCast(%camPos, %targetPosition, -1);
   
   %pos = EWCreatorWindow.getCreateObjectPosition();

   if(%rayResult != 0)
   {
      %pos = getWords(%rayResult, 1, 3);
   }
   else
   {
      %pos = "0 0 0";  
   }
   
   %newPrefab = new Prefab()
   {
      position = %pos;
      fileName = %assetDef;
   };
   
   getScene(0).add(%newPrefab);
   
   EWorldEditor.clearSelection();
   EWorldEditor.selectObject(%newPrefab);
      
   EWorldEditor.isDirty = true;
}