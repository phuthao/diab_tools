using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace DiaB.Core.Web.Helpers
{
    public static class FirebaseHelper
    {
        public static FirebaseApp InitializeFirebaseApp(string credentialFilePath)
        {
            return FirebaseApp.DefaultInstance ?? FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(credentialFilePath)
            });
        }

        public static FirebaseMessaging InitializeFirebaseMessaging(string credentialFilePath)
        {
            return FirebaseMessaging.GetMessaging(InitializeFirebaseApp(credentialFilePath));
        }
    }
}
