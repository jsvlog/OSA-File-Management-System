using System.Collections.Generic;

namespace OSA_File_Management_System.Model
{
    public static class BarangayData
    {
        public static Dictionary<string, List<string>> BarangaysByMunicipality = new Dictionary<string, List<string>>
        {
            ["Calapan"] = new List<string>
            {
                "Balingayan", "Balite", "Baruyan", "Batino", "Bayanan I", "Bayanan II", "Biga", "Bondoc", "Bucayao",
                "Buhuan", "Bulusan", "Calero", "Camansihan", "Canubing I", "Canubing II", "Comunal", "Guinimbangan",
                "Gulod", "Gutad", "Ibaba East", "Ibaba West", "Ilaya", "Lalud", "Lazareto", "Libis", "Lumangbayan",
                "Mahal na Pangalan", "Maidlang", "Malad", "Malamig", "Managpi", "Masipit", "Nag-Iba I", "Nag-Iba II",
                "Navotas", "Pachoca", "Palhi", "Panggalaan", "Parang", "Patas", "Personas", "Puting Tubig",
                "San Antonio", "San Rafael (Salong)", "San Vicente Central", "San Vicente East", "San Vicente North",
                "San Vicente South", "San Vicente West", "Sapul", "Silonay", "Sta. Cruz", "Sta. Isabel",
                "Sta. Maria Village", "Sta. Rita", "Sto. Niño", "Suqui", "Tawagan", "Tawiran", "Tibag", "Wawa"
            },
            ["Baco"] = new List<string>
            {
                "Catuwiran I", "Alag", "Burbuli", "Sta Rosa II", "Katuwiran II", "Sta Rosa I", "Mangangan I",
                "Dulangan I", "Mangangan II", "Mayabig", "Tagumpay", "Sta Cruz", "Pambisan", "Poblacion", "Tabon-tabon",
                "San Andres", "Putican-cabulo", "Pulantubig", "Water", "San Ignacio", "Lumangbayan", "Baras",
                "Bangkatan", "Bayanan", "Dulangan II", "Malapad", "Lantuyang"
            },
            ["San Teodoro"] = new List<string>
            {
                "Poblacion", "Caagutayan", "Tacligan", "Lumangbayan", "Calangatan", "Bigaan", "Calsapa", "Ilag"
            },
            ["Puerto Galera"] = new List<string>
            {
                "Aninuan", "Balatero", "Baclayan", "Dulangan", "Palangan", "Poblacion", "Sabang", "San Antonio",
                "San Isidro", "Sinandigan", "Sto. Nino", "Tabinay", "Villaflor"
            },
            ["Naujan"] = new List<string>
            {
                "Adrialuna", "Andres Ylagan", "Antipolo", "Apitong", "Arangin", "Aurora", "Bacungan", "Bagong buhay",
                "Balite", "Bancuro", "Banuton", "Buhangin", "Barcenaga", "Bayani", "Caburo", "Concepcion", "Dao",
                "Del pilar", "Estrella", "Evangelista", "Gamao", "General Esco", "Herrera", "Inarawan", "Kalinisan",
                "Laguna", "Mabini", "Magtibay", "Mahabang Parang", "Malaya", "Malinao", "Malvar", "Masagana",
                "Masaguing", "Melgar A", "Melgar B", "Metolza", "Montelago", "Montemayor", "Motoderazo", "Mulawin",
                "Nag-iba 1", "Nag-iba 2", "Pagkakaisa", "Paitan", "Paniquian", "Pinagsabangan 1", "Pinagsabangan 2",
                "Pinahan", "Poblacion 1", "Poblacion 2", "Poblacion 3", "Sampaguita", "Santiago", "San Agustin 1",
                "San Agustin 2", "San Andres", "San Antonio", "San Carlos", "San Isidro", "San Jose", "San Luis",
                "San Nicolas", "San Pedro", "Sta. Cruz", "Sta. Isabel", "Sta. Maria", "Sto. Nino", "Tagumpay", "Tigkan"
            },
            ["Victoria"] = new List<string>
            {
                "Alcalte", "Antonino", "Babangonan", "Bagong Buhay", "Bagong Silang", "Bambanin", "Bethel", "Canaan",
                "Concepcion", "Duongan", "Leido", "Loyal", "Mabini", "Macatoc", "Malabo", "Merit", "Ordovilla",
                "Pakyas", "Poblacion I", "Poblacion II", "Poblacion III", "Poblacion IV", "Sampaguita", "San Antonio",
                "San Cristobal", "San Gabriel", "San Gelacio", "San Isidro", "San Juan", "San Narciso", "Urdaneta",
                "Villa Cerveza"
            },
            ["Socorro"] = new List<string>
            {
                "Bagsok", "Batong Dalig", "Bayuin", "Bugtong na Tuog", "Calocmoy", "Calubayan", "Catiningan",
                "Fortuna", "Happy Valley", "Leuteboro I", "Leuteboro II", "Ma. Concepcion", "Mabuhay I", "Mabuhay II",
                "Malugay", "Matungao", "Monteverde", "Pasi I", "Pasi II", "Sto. Domingo", "Subaan", "Villareal",
                "Zone I", "Zone II", "Zone III", "Zone IV"
            },
            ["Pola"] = new List<string>
            {
                "Bacawan", "Bacungan", "Batuhan", "Bayanan", "Biga", "Buhay na Tubig", "Calima", "Calubasanhon",
                "Campamento", "Casiligan", "Malibago", "Maluanluan", "Matulatula", "Misong", "Pahilahan", "Panikihan",
                "Pula", "Puting Cacao", "Tagbakin", "Tagumpay", "Tiguihan", "Zone I", "Zone II"
            },
            ["Pinamalayan"] = new List<string>
            {
                "Anoling", "Bangbang", "Bacungan", "Banilad", "Buli", "Cacawan", "Calingag", "Del Razon", "Guinhawa",
                "Lumambayan", "Malaya", "Maliangcog", "Maningcol", "Marayos", "Marfrancisco", "Nabuslot", "Pagalagala",
                "Palayan", "Pambisan Malaki", "Pambisan Munti", "Panggulayan", "Papandayan", "Pili", "Quinabigan",
                "Ranzo", "Rosario", "Sabang", "Sta. Isabel", "Sta. Maria", "Sta. Rita", "Sto. Nino", "Wawa",
                "Zone 1", "Zone III", "Zone IV", "Inclanay", "Zone II"
            },
            ["Gloria"] = new List<string>
            {
                "Agos", "Agsalin", "Alma Villa", "Andres Bonifacio", "Balete", "Banus", "Banutan", "Bulaklakan",
                "Buong Lupa", "G. Antonio", "Guimbonan", "Kawit", "Lucio Laurel", "M. Adriatico", "Malamig",
                "Malayong", "Maligaya", "Malubay", "Manguyang", "Maragooc", "Mirayan", "Narra", "Papandungin",
                "San Antonio", "Sta. Maria", "Sta. Theresa", "Tambong"
            },
            ["Bansud"] = new List<string>
            {
                "Alcadesma", "Bato", "Conrazon", "Malo", "Manihala", "Pag-asa", "Poblacion", "Proper Bansud",
                "Proper Tiguisan", "Rosacara", "Salcedo", "Sumagui", "Villa Pag-asa"
            },
            ["Bongabong"] = new List<string>
            {
                "Anilao", "Aplaya", "BBI", "BBII", "Batangan", "Bukal", "Camantigue", "Carmundo", "Cawayan",
                "Dayhagan", "Formon", "Hagan", "Hagupit", "Ipil", "Kaligtasan", "Labasan", "Labonan", "Libertad",
                "Lisap", "Luna", "Malitbog", "Mapang", "Masaguisi", "Mina De Oro", "Morente", "Ogbot", "Orconuma",
                "Poblacion", "Pulosahi", "Sagana", "San Jose", "San Juan", "Sta. Cruz", "Sigange", "Tawas", "San Isidro"
            },
            ["Roxas"] = new List<string>
            {
                "Bagumbayan", "Cantil", "Dangay", "Happy Valley", "Libertad", "Libtong", "Little Tanauan", "Mabuhay",
                "Maraska", "Odiong", "Paclasan", "San Aquilino", "San Isidro", "San Jose", "San Mariano", "San Miguel",
                "San Rafael", "San Vicente", "Uyao", "Victoria"
            },
            ["Mansalay"] = new List<string>
            {
                "B. Del Mundo", "Balugo", "Bonbon", "Budburan", "Cabalwa", "Don Pedro", "Maliwanag", "Manaul",
                "Panaytayan", "Poblacion", "Roma", "Santa Brigida", "Santa Maria", "Santa Teresita", "Villa Celestial",
                "Wasig", "Waygan"
            },
            ["Bulalacao"] = new List<string>
            {
                "Bagong Sikat", "Balatasan", "Benli", "Cabugao", "Cambunang", "Campaasan", "Maasin", "Maujao",
                "Milagrosa", "Nasukob", "Poblacion", "San Francisco", "San Isidro", "San Juan", "San Roque"
            }
        };

        public static bool IsBarangayDocumentType(string docType)
        {
            return docType == "Barangay Financial Statement" ||
                   docType == "Barangay AOM" ||
                   docType == "Barangay AAR";
        }

        public static List<string> GetBarangaysForMunicipality(string municipality)
        {
            if (BarangaysByMunicipality.ContainsKey(municipality))
            {
                return BarangaysByMunicipality[municipality];
            }
            return new List<string>();
        }
    }
}
