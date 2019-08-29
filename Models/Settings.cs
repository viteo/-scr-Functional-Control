using System;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Sharpsaver
{
    public enum Layout
    {
        Straight,
        Brickwall
    }

    public enum Param2
    {
        Ei,
        Bi,
        Si,
        Di,
    }
}

namespace Sharpsaver.Models
{
    public class Settings
    {
        private readonly string settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
            "\\sharpsaver.xml";

        public static Settings Instance;
        public static Random Random = new Random();

        public Layout Layout { get; set; }
        public Param2 param2 { get; set; }
        public int BrickSize { get; set; }
        public double SwitchPeriod { get; set; }
        public bool ShowMagicNumber { get; set; }
        public bool IsFullscreen { get; set; }

        public Settings()
        {
            //Default Parameters
            this.Layout = Layout.Straight;
            this.param2 = Param2.Ei;
            this.BrickSize = 4;
            this.ShowMagicNumber = true;
            this.IsFullscreen = true;
        }

        public void LoadSettings()
        {
            try
            {
                // Create an instance of the Settings class
                Settings settings = new Settings();

                if (File.Exists(this.settingsPath))
                {
                    // Create an instance of System.Xml.Serialization.XmlSerializer
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                    // Create an instance of System.IO.StreamReader 
                    // to point to the settings file on disk
                    StreamReader textReader = new StreamReader(this.settingsPath);

                    // Create an instance of System.Xml.XmlTextReader
                    // to read from the StreamReader
                    XmlTextReader xmlReader = new XmlTextReader(textReader);

                    if (serializer.CanDeserialize(xmlReader))
                    {
                        // Assign the deserialized object to the new settings object
                        settings = ((Settings)serializer.Deserialize(xmlReader));

                        Instance = settings;
                    }

                    // Close the XmlTextReader
                    xmlReader.Close();
                    textReader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving settings!\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SaveSettings()
        {
            try
            {
                // Create an instance of System.Xml.Serialization.XmlSerializer
                XmlSerializer serializer = new XmlSerializer(this.GetType());

                // Create an instance of System.IO.TextWriter
                // to save the serialized object to disk
                TextWriter textWriter = new StreamWriter(this.settingsPath, false);

                // Serialize the settings object
                serializer.Serialize(textWriter, Instance);

                // Close the TextWriter
                textWriter.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving settings!\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
