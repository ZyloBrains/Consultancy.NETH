namespace Consultancy.Services;

public interface ILocalizationService
{
    string CurrentLanguage { get; set; }
    void SetLanguage(string lang);
    string Get(string key);
    string Get(string key, string lang);
}

public class LocalizationService : ILocalizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string LangKey = "CurrentLanguage";
    
    private static readonly Dictionary<string, Dictionary<string, string>> _translations = new()
    {
        ["en"] = new Dictionary<string, string>
        {
            ["Home"] = "Home",
            ["Courses"] = "Courses",
            ["Countries"] = "Countries",
            ["About Us"] = "About Us",
            ["Contact"] = "Contact",
            ["Blogs"] = "Blogs",
            ["Events"] = "Events",
            ["Teachers"] = "Teachers",
            ["Get Started"] = "Get Started",
            ["Featured Courses"] = "Featured Courses",
            ["All Courses"] = "All Courses",
            ["Study in"] = "Study in",
            ["Choose Top Destinations"] = "Choose Top Destinations",
            ["Featured Teachers"] = "Featured Teachers",
            ["Latest Events"] = "Latest Events",
            ["All Events"] = "All Events",
            ["Peoples Testimonial"] = "What people say about us",
            ["Latest News"] = "Latest News",
            ["Read More"] = "Read More",
            ["Our students come from every corner of the country"] = "Our students come from every corner of the country",
            ["Enroll Today"] = "Enroll Today",
            ["Quick Links"] = "Quick Links",
            ["Follow Us"] = "Follow Us",
            ["Name"] = "Name",
            ["Email"] = "Email",
            ["Phone"] = "Phone",
            ["Subject"] = "Subject",
            ["Message"] = "Message",
            ["Send Message"] = "Send Message",
            ["Your Name"] = "Your Name",
            ["Your Email"] = "Your Email",
            ["Your Phone"] = "Your Phone",
            ["Your Message"] = "Your Message",
            ["Submit"] = "Submit",
            ["Who we are"] = "Who we are",
            ["Prepare for your Abroad Career"] = "Prepare for your Abroad Career",
            ["Register Now"] = "Register Now",
            ["Start your Career with us!"] = "Start your Career with us!",
            ["You can start and finish one of these popular courses in under a day - for free!"] = "You can start and finish one of these popular courses in under a day - for free!",
            ["Fill The Form Now"] = "Fill The Form Now",
            ["Category"] = "Category",
            ["Duration"] = "Duration",
            ["Fees"] = "Fees",
            ["Description"] = "Description",
            ["Get Enroll"] = "Get Enroll",
            ["Search"] = "Search...",
            ["No courses found"] = "No courses found",
            ["No countries found"] = "No countries found",
            ["No events found"] = "No events found",
            ["No blogs found"] = "No blogs found"
        },
        ["np"] = new Dictionary<string, string>
        {
            ["Home"] = "होम",
            ["Courses"] = "कोर्सहरू",
            ["Countries"] = "देशहरू",
            ["About Us"] = "हाम्रो बारे",
            ["Contact"] = "सम्पर्क",
            ["Blogs"] = "ब्लगहरू",
            ["Events"] = "इभेन्टहरू",
            ["Teachers"] = "शिक्षकहरू",
            ["Get Started"] = "सुरु गर्नुहोस्",
            ["Featured Courses"] = "विशेष कोर्सहरू",
            ["All Courses"] = "सबै कोर्सहरू",
            ["Study in"] = "मा अध्ययन गर्नुहोस्",
            ["Choose Top Destinations"] = "शीर्ष गंतव्यहरू चुन्नुहोस्",
            ["Featured Teachers"] = "विशेष शिक्षकहरू",
            ["Latest Events"] = "नवीनतम इभेन्टहरू",
            ["All Events"] = "सबै इभेन्टहरू",
            ["Peoples Testimonial"] = "हाम्रो बारेमा मान्छेहरू के भन्छन्",
            ["Latest News"] = "ताजा खबर",
            ["Read More"] = "थप पढ्नुहोस्",
            ["Our students come from every corner of the country"] = "हाम्रा विद्यार्थीहरू देशको हरेक कुनाबाट आउँछन्",
            ["Enroll Today"] = "आज नै भर्ना गर्नुहोस्",
            ["Quick Links"] = "द्रुत लिंकहरू",
            ["Follow Us"] = "हामीलाई पछ्याउनुहोस्",
            ["Name"] = "नाम",
            ["Email"] = "इमेल",
            ["Phone"] = "फोन",
            ["Subject"] = "विषय",
            ["Message"] = "सन्देश",
            ["Send Message"] = "सन्देश पठाउनुहोस्",
            ["Your Name"] = "तपाईंको नाम",
            ["Your Email"] = "तपाईंको इमेल",
            ["Your Phone"] = "तपाईंको फोन",
            ["Your Message"] = "तपाईंको सन्देश",
            ["Submit"] = "पेश गर्नुहोस्",
            ["Who we are"] = "हामी को हो",
            ["Prepare for your Abroad Career"] = "विदेशमा आफ्नो करियरको लागि तयार हुनुहोस्",
            ["Register Now"] = "अहिले दर्ता गर्नुहोस्",
            ["Start your Career with us!"] = "हामीसँग आफ्नो करियर सुरु गर्नुहोस्!",
            ["You can start and finish one of these popular courses in under a day - for free!"] = "तपाईं यी लोकप्रिय कोर्सहरू मध्ये एक निःशुल्क एक दिनभित्र सुरु र समाप्त गर्न सक्नुहुन्छ!",
            ["Fill The Form Now"] = "अब फर्म भर्नुहोस्",
            ["Category"] = "श्रेणी",
            ["Duration"] = "अवधि",
            ["Fees"] = "शुल्क",
            ["Description"] = "विवरण",
            ["Get Enroll"] = "भर्ना गर्नुहोस्",
            ["Search"] = "खोज्नुहोस्...",
            ["No courses found"] = "कोर्स फेला परेन",
            ["No countries found"] = "देश फेला परेन",
            ["No events found"] = "इभेन्ट फेला परेन",
            ["No blogs found"] = "ब्लग फेला परेन"
        }
    };
    
    public LocalizationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string CurrentLanguage
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            return context?.Session.GetString(LangKey) ?? "en";
        }
        set
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Session.SetString(LangKey, value);
            }
        }
    }
    
    public void SetLanguage(string lang)
    {
        if (lang == "en" || lang == "np")
        {
            CurrentLanguage = lang;
        }
    }
    
    public string Get(string key)
    {
        return Get(key, CurrentLanguage);
    }
    
    public string Get(string key, string lang)
    {
        if (_translations.TryGetValue(lang, out var langDict))
        {
            if (langDict.TryGetValue(key, out var value))
            {
                return value;
            }
        }
        return key;
    }
}