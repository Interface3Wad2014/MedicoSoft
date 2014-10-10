using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// Class décrivant le medecin
    /// <seealso cref="DAL.Utilisateur"/>
    /// </summary>
    public class Medecin: Utilisateur
    {
        string _INAMI;
        #region Properties

        /// <summary>
        /// Numéro INAMI du medecin
        /// </summary>
        public string INAMI
        {
            get { return _INAMI; }
            set { _INAMI = value; }
        } 
        #endregion

        #region Function
        /// <summary>
        /// Permet de récupérer le medeci de la DB via son numéro INAMI
        /// </summary>
        /// <param name="Inami">Numéro INAMI du medecin</param>
        /// <returns></returns>
        public static Medecin getInfo(string Inami)
        {
            List<Dictionary<string, object>> infoUser = GestionConnexion.Instance.getData("Select * from Medecin where INAMI=" + Inami);
            Medecin retour = new Medecin();
            foreach (Dictionary<string, object> item in infoUser)
            {
                Utilisateur.getInfo(item["FKIdUtilisateur"].ToString(), retour as Utilisateur);
                retour.INAMI = Inami;
            }
            return retour;
        }

        /// <summary>
        /// Permet de sauvegarder le medecin dans la DB
        /// <seealso cref="Utilisateur.saveMe"/>
        /// </summary>
        /// <returns>true si le mdecein a pu être enregistré</returns>
        public override bool saveMe()
        {

            if (base.saveMe()) //Appel de l'enregistrement du parent (Utilisateur)... si el parent s'enregistre => poursuite
            {
                Medecin m = Medecin.getInfo(this.INAMI);
                string query = "";
                if (m == null)
                {
                    //Requête parametrée
                     query = @"INSERT INTO [MedicoDB].[dbo].[Medecin]
                                   ([INAMI]
                                   ,[FKIdUtilisateur])
                             VALUES
                                   (@INAMI,@FKIdUtilisateur)";
                }
                else
                {
                    query = @"UPDATE [MedicoDB].[dbo].[Medecin]
                                SET [FKIdUtilisateur] = @FKIdUtilisateur
                                WHERE [INAMI] = @INAMI";
                }

                //les données a insérer dans un dictionnaire
                Dictionary<string, object> valeurs = new Dictionary<string, object>();
                valeurs.Add("INAMI", this.INAMI);
                valeurs.Add("FKIdUtilisateur", this.IdUtilisateur); //idUtilisateur est récupéré du parent qui a été préalablement enregistré

                //Sauvegarde via la classe globale de gestion de données
                if (GestionConnexion.Instance.saveData(query, GenerateKey.APP, valeurs))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// Permet de vérifier si un utilisateur correspond a un Medecin
        /// </summary>
        /// <param name="u">L'utilisateur à vérifier</param>
        /// <returns>true si l'utilisateur est un medecin</returns>
        public static bool Exists(Utilisateur u)
        {
            List<Dictionary<string, object>> infoUser = GestionConnexion.Instance.getData("Select * from Medecin where FKIdUtilisateur=" + u.IdUtilisateur);

            return infoUser.Count > 0;
        } 
        #endregion
    }
}
