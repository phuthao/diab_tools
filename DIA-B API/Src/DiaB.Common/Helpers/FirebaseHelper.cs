using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace DiaB.Common.Helpers
{
    public static class FirebaseHelper
    {
        public static FirebaseMessaging InitFirebaseMessaging(string path)
        {
            if (FirebaseMessaging.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(path),
                });
            }

            return FirebaseMessaging.DefaultInstance;
        }
    }
}
