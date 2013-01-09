using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoService
{
    public static class DbInteraction
    {
        static string passwordCurrent = "technicise";
        static string dbmsCurrent = "photodb";

        public static MySql.Data.MySqlClient.MySqlConnection OpenDbConnection()
        {
            MySql.Data.MySqlClient.MySqlConnection msqlConnection = null;

            msqlConnection = new MySql.Data.MySqlClient.MySqlConnection("server=localhost;user id=root;Password=" + passwordCurrent + ";database=" + dbmsCurrent + ";persist security info=False");

            //open the connection
            if (msqlConnection.State != System.Data.ConnectionState.Open)
                msqlConnection.Open();

            return msqlConnection;
        }

        public static int AddPhoto(PhotoItem PhotoDetails)
        {
            int returnVal = 0;
            MySql.Data.MySqlClient.MySqlConnection msqlConnection = OpenDbConnection();

            try
            {
                //define the command reference
                MySql.Data.MySqlClient.MySqlCommand msqlCommand = new MySql.Data.MySqlClient.MySqlCommand();

                //define the connection used by the command object
                msqlCommand.Connection = msqlConnection;

                msqlCommand.CommandText = "INSERT INTO pics(PhotoID,Description,Name,UploadedOn) "
                                                   + "VALUES(@PhotoID,@Description,@Name,@UploadedOn)";

                msqlCommand.Parameters.AddWithValue("@PhotoID", PhotoDetails.PhotoID);
                msqlCommand.Parameters.AddWithValue("@Description", PhotoDetails.Description);
                msqlCommand.Parameters.AddWithValue("@Name", PhotoDetails.Name);
                msqlCommand.Parameters.AddWithValue("@UploadedOn", PhotoDetails.UploadedOn);
                msqlCommand.Parameters.AddWithValue("@ImgFile", PhotoDetails.ImgFile);

                msqlCommand.ExecuteNonQuery();

                returnVal = 1;
            }
            catch (Exception er)
            {
                returnVal = 0;
            }
            finally
            {
                //always close the connection
                msqlConnection.Close();
            }
            return returnVal;
        }

        public static PhotoItem GetPhoto(string PhotoToGet)
        {
            MySql.Data.MySqlClient.MySqlConnection msqlConnection = OpenDbConnection();
            PhotoItem thePhoto = null;

            try
            {   //define the command reference
                MySql.Data.MySqlClient.MySqlCommand msqlCommand = new MySql.Data.MySqlClient.MySqlCommand();
                msqlCommand.Connection = msqlConnection;

                msqlCommand.CommandText = "SELECT FROM  pics WHERE PhotoID=@PhotoToGet";
                MySql.Data.MySqlClient.MySqlDataReader msqlReader = msqlCommand.ExecuteReader();

                while (msqlReader.Read())
                {
                    thePhoto = new PhotoItem();

                    thePhoto.PhotoID = msqlReader.GetInt32("PhotoID");
                    thePhoto.Description = msqlReader.GetString("Description");
                    thePhoto.Name = msqlReader.GetString("Name");
                    thePhoto.UploadedOn = msqlReader.GetDateTime("UploadedOn");
                    thePhoto.FileSize = msqlReader.GetInt32("FileSize");
                    thePhoto.ImgFile = new byte[thePhoto.FileSize];
                    msqlReader.GetBytes(msqlReader.GetOrdinal("ImgFile"), 0, thePhoto.ImgFile, 0, thePhoto.FileSize); 
                }

            }
            catch (Exception er)
            {
            }
            finally
            {
                //always close the connection
                msqlConnection.Close();
            }
            return thePhoto;
        }



        public static List<PhotoItem> QueryAllPhotoList()
        {
            List<PhotoItem> photoList = new List<PhotoItem>();

            MySql.Data.MySqlClient.MySqlConnection msqlConnection = OpenDbConnection();

            try
            {   //define the command reference
                MySql.Data.MySqlClient.MySqlCommand msqlCommand = new MySql.Data.MySqlClient.MySqlCommand();
                msqlCommand.Connection = msqlConnection;

                msqlCommand.CommandText = "Select * From pics;";
                MySql.Data.MySqlClient.MySqlDataReader msqlReader = msqlCommand.ExecuteReader();

                while (msqlReader.Read())
                {
                    PhotoItem thePhoto = new PhotoItem();

                    thePhoto.PhotoID = msqlReader.GetInt32("PhotoID");
                    thePhoto.Description = msqlReader.GetString("Description");
                    thePhoto.Name = msqlReader.GetString("Name");
                    thePhoto.UploadedOn = msqlReader.GetDateTime("UploadedOn");
                    thePhoto.FileSize = msqlReader.GetInt32("FileSize");
                    thePhoto.ImgFile = new byte[thePhoto.FileSize];
                    msqlReader.GetBytes(msqlReader.GetOrdinal("ImgFile"), 0, thePhoto.ImgFile, 0, thePhoto.FileSize);  
                    photoList.Add(thePhoto);
                }

            }
            catch (Exception er)
            {
            }
            finally
            {
                //always close the connection
                msqlConnection.Close();
            }

            return photoList;
        }


        public static void DeletePhoto(string PhotoToDelete)
        {
            MySql.Data.MySqlClient.MySqlConnection msqlConnection = OpenDbConnection();

            try
            {   //define the command reference
                MySql.Data.MySqlClient.MySqlCommand msqlCommand = new MySql.Data.MySqlClient.MySqlCommand();
                msqlCommand.Connection = msqlConnection;

                msqlCommand.CommandText = "DELETE FROM pics WHERE PhotoID=@PhotoIdToDelete";
                msqlCommand.Parameters.AddWithValue("@PhotoIdToDelete", PhotoToDelete);

                MySql.Data.MySqlClient.MySqlDataReader msqlReader = msqlCommand.ExecuteReader();

            }
            catch (Exception er)
            {
            }
            finally
            {
                //always close the connection
                msqlConnection.Close();
            }

        }



    }
}