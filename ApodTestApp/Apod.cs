using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApodTestApp
{
    public class ApodData
    {
        public string copyright { get; set; }
        public string date { get; set; }
        public string explanation { get; set; }
        public string hdurl { get; set; }
        public string media_type { get; set; }
        public string service_version { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string thumbnail_url { get; set; }
    }

    class Apod
    {
        #region API Key

        private const string apiKey = "9FngbRpKNVGcpHkxpQJHzfDOw0NY7N71DHRW7XzI";

        #endregion

        #region Constants

        private const string baseUrl = "https://api.nasa.gov/planetary/apod?api_key=";
        private const string errorUrl = "https://placehold.jp/3d4070/ffffff/720x480.png?text=";
        private const string thumbsParameter = "&thumbs=true";
        private const string noImageMessage = "No Apod image for today";
        private const string noCopyright = "None";
        private const string dateParameter = "&date=";
        private const string startDateParameter = "&start_date=";
        private const string endDateParameter = "&end_date=";
        private const string countParameter = "&count=";
        #endregion

        #region Properties

        public string Description { get; private set; } = string.Empty;
        public string Information { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string LastError { get; private set; } = string.Empty;
        public bool HighDef { get; set; } = false;
        public string Date { get; set; } = string.Empty;
        public Uri LastUri { get; private set; } = null;

        #endregion

        #region members

        private HttpClient httpClient = new();

        private DateTime today;
        private readonly DateTime epoc;
        private DateTime lastDate;

        private ApodData currentApodData;

        private ApodData[] imagesArray;

        #endregion

        #region Constructor

        public Apod(bool highDef = false)
        {
            HighDef = highDef;

            epoc = new DateTime(1995, 06, 16); // first apod image date

            today = DateTime.Now;
        }

        #endregion

        #region Methods

        public async Task<Uri> GetApodUri()
        {
            today = DateTime.Now;
            lastDate = today;

            var request = new Uri($"{baseUrl}{apiKey}{thumbsParameter}");

            var success = await GetApod(request);

            if (success)
            {
                return GetValidUri();
            }
            else
            {
                return new Uri($"{errorUrl}{LastError}");
            }

        }

        private Uri GetValidUri()
        {
            if (currentApodData.media_type == "image")
            {
                if (HighDef && currentApodData.hdurl != null)
                {
                    LastUri = new Uri(currentApodData.hdurl);
                }
                else
                {
                    LastUri = new Uri(currentApodData.url);
                }
            }
            else if (currentApodData.media_type == "video" &&
                !string.IsNullOrEmpty(currentApodData.thumbnail_url))
            {
                LastUri = new Uri(currentApodData.thumbnail_url);
            }
            else
            {
                LastError = $"No valid URL for {currentApodData.date}";
                LastUri = new Uri($"{errorUrl}{noImageMessage}");
            }

            return LastUri;
        }

        private async Task<bool> GetApod(Uri request)
        {
            try
            {
                currentApodData = await httpClient.GetFromJsonAsync<ApodData>(request);
                SetInformationAndDescription();
                return true;
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return false;
            }
        }

        private void SetInformationAndDescription()
        {
            Title = currentApodData.title;
            Date = currentApodData.date;

            if (string.IsNullOrEmpty(currentApodData.copyright))
            {
                currentApodData.copyright = noCopyright;
            }

            Description =
                $"{currentApodData.title} (\u00A9 {currentApodData.copyright})";

            Information = $"{currentApodData.explanation}{Environment.NewLine}" +
                $"\u00A9 {currentApodData.copyright}{Environment.NewLine}" +
                $"{Environment.NewLine}Image Date: {currentApodData.date}";
        }

        // get number of random images
        public async Task<ApodData[]> GetNumberOfImages(int number)
        {
            var request = new Uri($"{baseUrl}{apiKey}{thumbsParameter}{countParameter}{number}");


            var responseArray = await httpClient.GetFromJsonAsync<ApodData[]>(request);

            // Error handling will be done somewhere else

            return imagesArray = responseArray;

        }

        // get images from start date and end date

        private async Task<ApodData[]> GetImagesByDateRange(DateTime startDate, DateTime endDate)
        {

            var startDateFormatted = startDate.ToString("yyyy-MM-dd");
            var endDateFormatted = endDate.ToString("yyyy-MM-dd");

            var request = new Uri($"{baseUrl}{apiKey}{startDateParameter}{startDateFormatted}{endDateParameter}{endDateFormatted}{thumbsParameter}");

            var responseArray = await httpClient.GetFromJsonAsync<ApodData[]>(request);

            // Error handling will be done somewhere else
            return imagesArray = responseArray;
        }        
        
        // get images by start date
        private async Task<ApodData[]> GetImagesByStartDate(DateTime startDate)
        {

            var startDateFormatted = startDate.ToString("yyyy-MM-dd");

            var request = new Uri($"{baseUrl}{apiKey}{startDateParameter}{startDateFormatted}{thumbsParameter}");

            var responseArray = await httpClient.GetFromJsonAsync<ApodData[]>(request);

            // Error handling will be done somewhere else
            return imagesArray = responseArray;
        }

        // get previous image
        public async Task<Uri> GetPreviousUri()
        {
            lastDate = lastDate.AddDays(-1);
            return await GetApodUriByDate(lastDate);
        }

        // get next image
        public async Task<Uri> GetNextUri()
        {
            lastDate = lastDate.AddDays(1);
            return await GetApodUriByDate(lastDate);
        }

        private async Task<Uri> GetApodUriByDate(DateTime newDate)
        {
            today = DateTime.Now;

            if (newDate > today)// can't get images from the future 
            {
                newDate = today;
            }
            else if (newDate < epoc) // can't get images before Apod existed!
            {
                newDate = epoc;
            }

            lastDate = newDate;

            var date = newDate.ToString("yyyy-MM-dd");

            var request = new Uri($"{baseUrl}{apiKey}{dateParameter}{date}{thumbsParameter}");

            var success = await GetApod(request);

            if (success)
                return GetValidUri();
            else
                return new Uri($"{errorUrl}{LastError}");
        }



    }
    #endregion

}
