using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Google.Apis.Services;
using Grpc.Auth;

namespace GoogleSpeechApp
{
  class Program
  {
    static void Main(string[] args)
    {
      var folderPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString(), @"Sounds\");
      var credentialsFilePath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString(), @"google-credentials.json");
      GoogleCredential googleCredential;
      using(Stream m = new FileStream(credentialsFilePath, FileMode.Open))
        googleCredential = GoogleCredential.FromStream(m);
      var channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.Host,
        googleCredential.ToChannelCredentials());
      var speech = SpeechClient.Create(channel);

      for(int t = 0; t < Directory.GetFiles(folderPath).ToList().Count; t++)
      {
        var currentFile = Directory.GetFiles(folderPath)[t];
        RecognizeSpeech(speech, currentFile);
      }
      
      Console.WriteLine("Done!");
      Console.Read();
    }

    static void RecognizeSpeech(SpeechClient speech, string currentFile)
    {
      var response = speech.Recognize(new RecognitionConfig()
      {
        LanguageCode = "nl-NL",
        // LanguageCode = "en-US",
      }, RecognitionAudio.FromFile(currentFile));
      foreach(var result in response.Results)
      {
        foreach(var alternative in result.Alternatives)
        {
          Console.WriteLine(alternative.Transcript);
        }
      }
    }
  }
}
