using Highgeek.McWebApp.Common.Models.Adapters.EcoData;
using Highgeek.McWebApp.Common.Models.Minecraft;
using Highgeek.McWebApp.Common.Data;
using Highgeek.McWebApp.Common.Models.mcserver_datadb;
using Highgeek.McWebApp.Common.Models.mcserver_ecodata;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Highgeek.McWebApp.Common.Services
{
    public class UUID
    {
        public static byte[] FromString(string uuid)
        {
            var guid = new Guid(uuid).ToByteArray();
            Array.Reverse(guid, 6, 2);
            Array.Reverse(guid, 4, 2);
            Array.Reverse(guid, 0, 4);
            return guid;
        }

        public static string FromBytes(byte[] uuid)
        {
            Array.Reverse(uuid, 0, 4);
            Array.Reverse(uuid, 4, 2);
            Array.Reverse(uuid, 6, 2);
            return new Guid(uuid).ToString();
        }
    }
    public class EcoPlayer
    {
        public EcoDataAdapter? EcoDatum { get; set; }
        public string? Playername { get; set; }
        public string? Id { get; set; }
        public List<Talismans>? Talismans { get; set; }
        public EcoPets? Pets { get; set; }
    }
    public class Talismans
    {
        public string Name { get; set; }
        public string Texture { get; set; }
    }
    public class EcoParser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly McserverMaindbContext _mcMainDbContext;
        private readonly McserverDatadbContext _mcDataDbContext;
        private readonly MineskinApiCommunication _mineskinApi;
        private readonly SkinManager _skinmanager;
        private readonly PteroManager _pteroManager;
        private readonly UsersDbContext _usersdb;
        private readonly McserverEcoDataContext _ecodb;
        private readonly ILogger<MinecraftUserManager> _logger;
        public EcoParser(
            UserManager<ApplicationUser> userManager,
            McserverMaindbContext mainDbContext,
            MineskinApiCommunication mineskinApiCommunication,
            SkinManager skinManager,
            McserverDatadbContext DataDbContext,
            PteroManager pteroManager,
            UsersDbContext usersdb,
            McserverEcoDataContext ecodb,
            ILogger<MinecraftUserManager> logger)
        {
            _userManager = userManager;
            _mcMainDbContext = mainDbContext;
            _mcDataDbContext = DataDbContext;
            _mineskinApi = mineskinApiCommunication;
            _skinmanager = skinManager;
            _pteroManager = pteroManager;
            _usersdb = usersdb;
            _logger = logger;
            _ecodb = ecodb;
        }


        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        public async Task<List<EcoPlayer>> GetNamesWithIdAndProperties()
        {
            List<EcoPlayer> result = new List<EcoPlayer>();
            //EcoDataDeserializer EcoDataDeserializer;
            var ecoData = await _ecodb.EcoData.ToListAsync();
            foreach (var ecoDatum in ecoData)
            {
                EcoPlayer ecoplayer = new EcoPlayer();
                EcoPlayer ecoplayeriterattion = EcoPlayerInicializer(ecoDatum, ecoplayer);
                if (ecoplayeriterattion == null)
                {
                    continue;
                }
                ecoplayer = ecoplayeriterattion;
                result.Add(ecoplayer);
            }
            return result;
        }


        public async Task<EcoPlayer> GetEcoDataForPlayer(byte[] id)
        {
            EcoPlayer ecoPlayer = new EcoPlayer();
            //EcoDataDeserializer EcoDataDeserializer;
            var ecoDatum = await _ecodb.EcoData.FirstOrDefaultAsync(s => s.Id == id);
            ecoPlayer = EcoPlayerInicializer(ecoDatum, ecoPlayer);
            return ecoPlayer;
        }


        public EcoPlayer EcoPlayerInicializer(EcoDatum ecoDatum, EcoPlayer ecoPlayer)
        {
            var deserialized = EcoDataAdapter.FromJson(ecoDatum.JsonData);
            if (deserialized == null)
            {
                return null;
            }
            if (deserialized.EcoPlayerName == null)
            {
                return null;
            }
            ecoPlayer.Pets = GetPlayerPets(deserialized);
            if (deserialized.TalismansTalismanBag != null)
            {
                ecoPlayer.Talismans = new List<Talismans>();
                foreach (var talisman in deserialized.TalismansTalismanBag)
                {
                    Talismans talismans = new Talismans();
                    string encoded = getBetween(talisman, " texture:", " name:");

                    byte[] data = Convert.FromBase64String(encoded);
                    string decodedString = System.Text.Encoding.UTF8.GetString(data);
                    decodedString = getBetween(decodedString, "ft.net/texture/", "\"}}}");
                    talismans.Texture = decodedString;
                    talismans.Name = getBetween(talisman, "talismans:", " texture");
                    ecoPlayer.Talismans.Add(talismans);
                }
            }
            //Guid guid = new Guid(ecoDatum.Id);
            //ecoPlayer.Id = guid.ToString();
            ecoPlayer.Id = ecoDatum.Id.ToString();
            ecoPlayer.EcoDatum = deserialized;
            ecoPlayer.Playername = deserialized.EcoPlayerName;
            return ecoPlayer;
        }
        public async Task<EcoPlayer> GetEcoPlayer(string uuid)
        {
            byte[] bytearray = UUID.FromString(uuid);
            var ecoDatum = await _ecodb.EcoData.FirstOrDefaultAsync(s => s.Id == bytearray);
            EcoPlayer ecoplayer = new EcoPlayer();
            ecoplayer = EcoPlayerInicializer(ecoDatum, ecoplayer);
            return ecoplayer;
        }

        public EcoPets GetPlayerPets(EcoDataAdapter ecoData)
        {
            if (ecoData == null)
            {
                return null;
            }
            var ecoPets = new EcoPets();
            ecoPets.Pets = new List<Pet>();
            if (ecoData.EcopetsSkeletonXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Skeleton";
                pets.Texture = "9d46eb642dc3a4dfbb5ad5297edae2996ea4cfff92ac2eb56dfae9ee1d58e408";
                pets.Xp = ecoData.EcopetsSkeletonXp.ToString();
                pets.Level = ecoData.EcopetsSkeletonLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            if (ecoData.EcopetsBlazeXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Blaze";
                pets.Texture = "b20657e24b56e1b2f8fc219da1de788c0c24f36388b1a409d0cd2d8dba44aa3b";
                pets.Xp = ecoData.EcopetsBlazeXp.ToString();
                pets.Level = ecoData.EcopetsBlazeLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            if (ecoData.EcopetsMancubusXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Mancubus";
                pets.Texture = "9095fcc1e3d7cbd350f19b389498ab8bb96c65ad185d34592067a7d033ac48de";
                pets.Xp = ecoData.EcopetsMancubusXp.ToString();
                pets.Level = ecoData.EcopetsMancubusLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            if (ecoData.EcopetsRavagerXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Ravager";
                pets.Texture = "cd20bf52ec390a0799299184fc678bf84cf732bb1bd78fd1c4b441858f0235a8";
                pets.Xp = ecoData.EcopetsRavagerXp.ToString();
                pets.Level = ecoData.EcopetsRavagerLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            if (ecoData.EcopetsSeaSerpentXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Sea Serpent";
                pets.Texture = "9095fcc1e3d7cbd350f19b389498ab8bb96c65ad185d34592067a7d033ac48de";
                pets.Xp = ecoData.EcopetsSeaSerpentXp.ToString();
                pets.Level = ecoData.EcopetsSeaSerpentLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            if (ecoData.EcopetsTigerXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Tiger";
                pets.Texture = "9095fcc1e3d7cbd350f19b389498ab8bb96c65ad185d34592067a7d033ac48de";
                pets.Xp = ecoData.EcopetsTigerLevel.ToString();
                pets.Level = ecoData.EcopetsTigerLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            if (ecoData.EcopetsUnicornXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Unicorn";
                pets.Texture = "70b33b7a3b24f76bd80a65c3a933e7188be358487342be143c22a7580b4d00e1";
                pets.Xp = ecoData.EcopetsUnicornXp.ToString();
                pets.Level = ecoData.EcopetsUnicornLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            if (ecoData.EcopetsVampireXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Vampire";
                pets.Texture = "3820a10db222f69ac2215d7d10dca47eeafa215553764a2b81bafd479e7933d1";
                pets.Xp = ecoData.EcopetsVampireXp.ToString();
                pets.Level = ecoData.EcopetsVampireLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            if (ecoData.EcopetsRhinoXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Rhino";
                pets.Texture = "1737b8aef42d88fa7fb68ad2d7bbefa4968f683ac70ec55122a14e74789aeb09";
                pets.Xp = ecoData.EcopetsRhinoXp.ToString();
                pets.Level = ecoData.EcopetsRhinoLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            if (ecoData.EcopetsJackolanternXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Jack o'lantern";
                pets.Texture = "5684f11480c0fc97480ed1b70c23139bb4fcd616cc967e6c779ba1d2ed55";
                pets.Xp = ecoData.EcopetsJackolanternXp.ToString();
                pets.Level = ecoData.EcopetsJackolanternLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            if (ecoData.EcopetsCheetahXp > 0)
            {
                var pets = new Pet();
                pets.Name = "Cheetah";
                pets.Texture = "9095fcc1e3d7cbd350f19b389498ab8bb96c65ad185d34592067a7d033ac48de";
                pets.Xp = ecoData.EcopetsCheetahXp.ToString();
                pets.Level = ecoData.EcopetsCheetahLevel.ToString();
                ecoPets.Pets.Add(pets);
            }
            return ecoPets;
        }
    }
}
