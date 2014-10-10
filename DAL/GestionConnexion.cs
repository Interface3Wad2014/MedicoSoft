using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// Enumération utilisée pour définir le type de génération pour les clés
    /// </summary>
    public enum GenerateKey { 
        /// <summary>
        /// Clé générée par la DB
        /// </summary>
        DB, 
        /// <summary>
        /// Clé calculée par l'application
        /// </summary>
        APP };
    /// <summary>
    /// Class de gestion de la connexion(connect, select, update,...)
    /// </summary>
    public class GestionConnexion
    {
       
        #region Singleton
        private static GestionConnexion _instance = null;

            /// <summary>
            /// Singleton Pattern. Représente une instance de l'objet GestionConnexion     
            /// </summary>
            public static GestionConnexion Instance
            {
                get { return _instance = _instance ?? new GestionConnexion(); }
            }
        #endregion

        #region Fields
        private SqlConnection _oConn;
        private SqlCommand _oCom;
        private string _cnstr = DAL.Properties.Settings.Default.ConnectionString;
        #endregion

       
        #region Function
        /// <summary>
        /// Permet de connecter la DB et d'initialiser l'objet command
        /// <code>
        ///  if (connectDb())
        ///    {
        ///    ...
        ///    }
        /// </code>
        /// </summary>
        /// <returns>true si la connexion a pu se faire</returns>
        private bool connectDb()
        {
                 _oConn = new SqlConnection(_cnstr);
                    try
                    {
                        _oConn.Open();
                        _oCom = new SqlCommand();
                        _oCom.Connection = _oConn;
                        return true;
                    }
                    catch (Exception)
                    {

                        throw new InvalidOperationException("La connexion à la db à échoué");
                    }
        }
        /// <summary>
        /// Récupère les données suite à une requête passée en paramètre
        /// <code>
        ///  List&lt;Dictionary&lt;string, object&gt;&gt; infoUser = GestionConnexion.Instance.getData("Select * from Medecin where INAMI=" + Inami);
        /// </code>
        /// </summary>
        /// <param name="cmd">la requête à exécuter</param>
        /// <returns>une liste de dictionnaire composés du nom du champs (key) et de sa valeur (value)</returns>
        public List<Dictionary<string, object>> getData(string cmd)
        {
            List<Dictionary<string, object>> DicoRet = new List<Dictionary<string, object>>();
            if (connectDb())
            {
                _oCom.CommandText= cmd;
                SqlDataReader odr = _oCom.ExecuteReader();
                if (odr.HasRows)
                {
                    while(odr.Read())
                    {
                        Dictionary<string, object> temp = new Dictionary<string, object>();
                        for (int i = 0; i < odr.FieldCount; i++)
                        {
                            string nomColonne = odr.GetName(i);//Permet de récupérer le nom de la colonne
                            object Valeur = odr[i];//récupère la valeur du champs en DB
                            temp.Add(nomColonne, Valeur);//ajout de la clé (nom du champs) et de la valeur (Valeur) dans le dictionnaire
                        }
                        DicoRet.Add(temp);//ajout du dictionnaire dans la lsite de retour
                    }
                }
                odr.Close();//fermeture du DataReader

            }
            _oConn.Close();
            return DicoRet;

        }
        /// <summary>
        /// Permet de sauvegarder les données en DB à partir d'un requête et d'un dictionnaire contenant le nom des paramètres (Key) et sa valeur (value)
        /// <code> 
        /// string query = "INSERT INTO [MedicoDB].[dbo].[Medecin]
        ///                           ([INAMI]
        ///                           ,[FKIdUtilisateur])
        ///                     VALUES
        ///                           (@INAMI,@FKIdUtilisateur)";
        ///
        ///            //les données a insérer dans un dictionnaire
        ///            Dictionary&lt;string, object&gt; valeurs = new Dictionary&lt;string, object&gt;();
        ///            valeurs.Add("INAMI", this.INAMI);
        ///            valeurs.Add("FKIdUtilisateur", this.IdUtilisateur); //idUtilisateur est récupéré du parent qui a été préalablement enregistré
        ///
        ///            //Sauvegarde via la classe globale de gestion de données
        ///            if (GestionConnexion.Instance.saveData(query, GenerateKey.APP, valeurs))
        ///            {
        ///                return true;
        ///            }
        ///            else
        ///            {
        ///                return false;
        ///            }
        /// </code>
        /// </summary>
        /// <param name="Query">Requête parametrée exemp: insert into table values (@champ)</param>
        /// <param name="gen">Permet de spécifier si la clé primaire est autogénéré ou pas. Si elle est autogénéré, on ne ferme pas la connexion pour permettre l'appel 
        /// à la fonction getLastGenerateId
        /// </param>
        /// <param name="valeurs">Dictionnaire contenant le nom des paramètres (Key) et leur valeur (value)</param>
        /// <returns>True si la requête a pu s'effectuer</returns>
        public bool saveData(string Query, GenerateKey gen, Dictionary<string, object> valeurs)
        {
            if (connectDb())
            {
                //parcours des valeurs et ajout comme pramètre dans la commande
                foreach (KeyValuePair<string, object> item in valeurs)
                {
                    SqlParameter param = new SqlParameter(item.Key, item.Value);//Création d'un paramètre pour la commande
                    _oCom.Parameters.Add(param);//ajout du paramètre à la commande
                }
                _oCom.CommandText = Query;//Attribution de la requête à la commande
                try
                {
                    _oCom.ExecuteNonQuery();
                    if (gen == GenerateKey.APP) _oConn.Close(); //Si nous sommes face à une clé non générée par la DB on ferme la connexion
                    return true;
                }
                catch (Exception)
                {
                    _oConn.Close();
                    return false;
                }
                
            }
            else
            {

                return false;
            }
        }
        /// <summary>
        /// Permet de récupérer le dernier ID généré par la db en int.
        /// Attention! l'appel a cette fonction ne peut se faire que si une insertion a été effectué au préalable
        /// par la fonction saveData
        /// <code>
        ///  if (GestionConnexion.Instance.saveData(query, GenerateKey.DB, valeurs))
        ///            {
        ///                //Appel de la fonction permettant de récupérer le dernier id généré
        ///                // par la base de données
        ///              this.IdUtilisateur= GestionConnexion.Instance.getLastGenerateId();
        ///                return true;
        ///            }
        ///            else
        ///            {
        ///                return false;
        ///            }
        /// </code>
        /// </summary>
        /// <returns>le dernier ID généré par la db en int</returns>
        /// 
        public int getLastGenerateId()
        {
            int retour = 0;
            if (_oConn.State== System.Data.ConnectionState.Open)
            {
                _oCom.CommandText = "Select @@identity";
                var tempretour =_oCom.ExecuteScalar();
                retour = int.Parse(tempretour.ToString());
            }

                 _oConn.Close();
                 return retour;
        }
        #endregion





       
    }
}
