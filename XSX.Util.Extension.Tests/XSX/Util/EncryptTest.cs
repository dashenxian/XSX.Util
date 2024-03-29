﻿using Shouldly;
using XSX.Util;
using Xunit;

namespace Tests.XSX.Util
{
    public class EncryptTest
    {
        [Fact]
        public void AESEncryptIVTest()
        {
            var key = "rwe23adasfadfaed";
            var iv = "czxvzcvaewfdfa23";
            var data = "{\"inputs\":[{\"files\":[\"C:\\\\Users\\\\Administrator\\\\Desktop\\\\3dloupan\\\\510521999999GB00127 (2)\\\\base_name_0.obj\"],\"name\":\"\",\"srs\":\"ENU:39.90691,116.39123\",\"srsorigin\":\"0,0,0\",\"towgs84\":\"\",\"encodeGBK\":true,\"flipYZ\":true,\"reverseTriangle\":false,\"forceDoubleSided\":false,\"nolight\":false,\"alphaMode\":\"\",\"props\":\"\",\"type\":\"OBJ\"}],\"database\":{\"sqlite\":{\"file\":\"\"},\"skipImport\":false},\"nolod\":{\"cmptMaxSize\":2000000,\"b3dmMaxSize\":20000000},\"i3dm\":{\"minRefCount\":50},\"lod\":{\"minZoom\":16,\"maxZoom\":19,\"threadcount\":4,\"filterPixelSize\":25,\"maxZoomRecon\":false,\"maxOpaqueTextureSize\":1024,\"maxTransparentTextureSize\":256,\"recon\":{\"enable\":true,\"keepSrcMesh\":false}},\"filters\":{\"simplify\":{\"exclude\":true},\"textureproj\":{\"exclude\":true},\"lod\":{\"exclude\":false}},\"color2texture\":false,\"dracoCompression\":false,\"compressLevel\":1,\"DracoAttributeQuantization\":{\"position\":14,\"tex_coord\":12,\"normal\":10,\"color\":8,\"genericAtt\":16},\"mergeRepeat\":false,\"textureFormat\":\"jpgorpng\",\"modelname\":false,\"packUnselectedProperty\":false,\"props\":[],\"firstsrs\":false,\"output\":{\"path\":\"C:\\\\Users\\\\Administrator\\\\Desktop\\\\3dloupan\\\\510521999999GB00127 (2)\\\\output\",\"type\":\"file\",\"outPutb3dm\":false},\"scenetree\":true,\"fields\":[]}";
            var result = Encrypt.AESEncryptHex(data, key, iv);

            #region exceptResult
            var exceptResult = "BD2F93B0BEB8A8646242F6B2DFEE35993354D56DE8E9429C7A82A1F4427DFCC43C55009B42D89F16924686C3E5BC754DE871B4CC28633C4699CB3B625FA3BACE1507755115274FDE0F0CB4A4EEBE8543643E19AF839930FDAC8BAED122B53A8744F99EC184B309EACD6C2DA48F020FF6DECE7030AFC9FA63D3889B7B3F88E5BDB207D5F3400AB7E424731B35B4565ADDEA28452BF92C7C5B12084D4791038399014E177EE5D175E127FA3DE2F09FE880B661659AE173D3F263E3AB61EF0F460053DE9F4E491A67B004DF69FF7D51D067A7CF581A332C9C06067811CF4263BF6D0C69AE28E05B3D94B4964857EB211D53CB3D2247E60F11EFB163DB050317AAE82B969605B701563E8F43A968AD29561ABA5F7066D40034AC011E6999B6BB3A3FDF0A42B2C9B5960AFE912BDE3E40F3E54C9C86DA124C96EB390E91B944A642A5DF313C771D39C3CD41D0CFFD1F6898BC7F94A4B0BB652B8C47D0B4DBC85933EC5E8A1C656B358D59F271D91355AAA32FDC6BEEB673C735AE737680AC16479F755AA06DEFA2AAE806F62E4A0485FD6BAF483606DD5D2C1ABFA1B072F75A3D240AFAFB7EC10F7438F9B02EED18A36CF156AFD8A8342D7AF5B520BE2D6658122E3697DBE78710BFF807E3BD3A20330BD61912B20C541FCA7E150308C0D762B91D1555B356B27998DF9066FAB45E2FED2F104B1A3AE7B5E98FA3D2DC6C82AA81BB5482008A18D64CDDCB4AC940662B7211B65E98A512F65CA0510C74FF49CC4CF9FD213C645499434FD352A0196A3355E3BD1904112797AC0C35AA5F6027696BBC1E346228E5F8D32BEBB343D5B73C6A57E652C2ACFECB795E09E19BD118F2563F004C8340AC5B5057F459100D1B480D937C9FE26095A78D72D73FFD1B565718F940461AF392F06BECA03AA98F174E5EF61349D00C2E2380CC5A3C616AA68FB6A20236E7199C851C4F5994888A8808FDC631B11A6F924905D829E517D144F597E6598E99BF50CFF8616B649528A995B6A13A208965D7853AEAB67F1D28B7F1178A4556CCDB9F3DCFE2D82104B845C5EE66051F37B62A87E7C2980466A13B816D375D361DCAA23D53EC080109F9BC383DF559E62FB8CB7F403F4FB3FD04760022419E8384E40E7E75B3D4721137798509C7BF48A3B89CBD637C7D361684EEFC2C64C9C0FB0F92D7A1F7A59F753F95F2D6D0C56C89806FFAE3A6EA04118F6F27D9428529F73F8549F6049939820A88120A9E701C608DCF799F35846E946140DA6707D1DDE254131AFC1BCE92879BA9788515204B0C214611D5283ACFF5A85CC0502EB7FCF9CF70F8E62F451D63927D1F530D3CBE76E595BC866A1FCADC4CABA716F7D8F4E4EE919941C23F4573430D122714D315B0E636396DEE4E7B904B1F93094813572D8C2B60FE84EDD2AF5F231C8B00ECE2039369AA64B47CEEEA5E8F703EAFF20A43E244B462ED52B2CE2EECD717A982443B1EC722EC9DA8F627C6A25C15F172855584821BBF214999944BDED76F3688ACCAAE09CBA5C829388BA5CCC9EDF5119D0EDC0BC35C33F02CF4B540142FBAD21ACC351B8CBC7E342BED801B2E660C9B8D9BD2A5E17638C2D7ACBBE677A936BFD37EC4F10744217BDEE439387BB116783CE862FC8837D7644ACD1CC5A87C983CDDB7EA8110C9409B27FE10AB28C8DFA0C9C4453AF55D6874199BD4C1D8889614";
            #endregion

            result.ShouldBe(exceptResult);
        }
        [Fact]
        public void AESDecryptIvTest()
        {

        }
    }
}
