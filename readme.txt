									Unity中AssetBundle压缩格式验证与分析
Unity Version: 5.3.6f1
Unity中Asset Bundles 支持三种压缩选项：
1.LZMA -- BuildAssetBundleOptions.None
2.LZ4 -- BuildAssetBundleOptions.ChunkBasedCompression
3.UnCompressed -- BuildAssetBundleOptions.UncompressedAssetBundle

Size：LZMA > LZ4 > UnCompressed 
loading： UnCompressed > LZ4 > LZMA

size:
BuildTarget.Android
5个AB total size：
LZMA: 3.53M
LZ4: 6.34M
UnCompressed: 27M

loading：
PC：
		LoadFromFile	WWW
LZMA : 		0.406
LZ4			0.015
UC..		0.004

Android：HM2A
		LoadFromFile	WWW
LZMA : 		
LZ4			
UC..		


分析：
