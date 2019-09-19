//Simple GET request

using System.Net;

...

using (var wb = new WebClient())
{
    var response = wb.DownloadString(url);
}





//Simple POST request

using System.Net;
using System.Collections.Specialized;

...

using (var wb = new WebClient())
{
    var data = new NameValueCollection();
    data["username"] = "myUser";
    data["password"] = "myPassword";

    var response = wb.UploadValues(url, "POST", data);
}