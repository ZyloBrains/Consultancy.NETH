# Consultancy Website Architecture Plan

## 1. Project Overview

- **Project Name:** Consultancy.NETH (Education Consultancy Website)
- **Technology Stack:** ASP.NET Core 8 (Razor Pages) + Entity Framework Core + SQL Server + Tailwind CSS
- **Language Support:** English & Nepali
- **Admin Panel:** Full backend management

---

## 2. Data Model (EF Core Entities)

### 2.1 Core Entities

```
┌─────────────────┐     ┌─────────────────┐
│     Course      │     │    Country     │
├─────────────────┤     ├─────────────────┤
│ Id (PK)         │     │ Id (PK)         │
│ Name            │     │ Name           │
│ Slug            │     │ Slug           │
│ Description     │     │ FlagImage      │
│ Image           │     │ Description    │
│ CategoryId (FK) │     │ Universities   │
│ CountryId (FK) │────▶│ CostOfLiving    │
│ Duration        │     │ VisaInfo       │
│ Fees            │     │ WorkPermit     │
│ IsFeatured      │     └─────────────────┘
│ DisplayOrder    │
│ IsActive        │
└─────────────────┘
        │
        │ 1:N
        ▼
┌─────────────────┐
│    Category     │
├─────────────────┤
│ Id (PK)         │
│ Name (EN)       │
│ Name (NP)       │
│ Slug            │
└─────────────────┘

┌─────────────────┐     ┌─────────────────┐
│     Teacher     │     │    Student     │
├─────────────────┤     ├─────────────────┤
│ Id (PK)         │     │ Id (PK)         │
│ Name            │     │ Name           │
│ Designation    │     │ Email          │
│ Photo          │     │ Phone          │
│ Bio             │     │ CourseId (FK)  │
│ Facebook        │     │ CountryId (FK) │
│ Instagram       │     │ Message        │
│ LinkedIn        │     │ Status         │
│ DisplayOrder    │     │ CreatedAt      │
│ IsActive        │     └─────────────────┘
└─────────────────┘
        │
        │ N:M (via CourseTeacher)
        ▼
┌─────────────────────┐
│  CourseTeacher     │
├─────────────────────┤
│ CourseId (FK, PK)  │
│ TeacherId (FK, PK) │
└─────────────────────┘

┌─────────────────┐     ┌─────────────────┐
│      Blog       │     │     Event      │
├─────────────────┤     ├─────────────────┤
│ Id (PK)         │     │ Id (PK)        │
│ Title           │     │ Title         │
│ Slug            │     │ Slug          │
│ ShortDescription│    │ Description   │
│ Content         │     │ Image        │
│ Image           │     │ Location     │
│ Author          │     │ EventDate    │
│ CreatedAt       │     │ CreatedAt    │
│ IsActive        │     │ IsActive     │
└─────────────────┘     └─────────────────┘

┌─────────────────┐     ┌─────────────────┐
│  Testimonial   │     │  Contact inquiry│
├─────────────────┤     ├─────────────────┤
│ Id (PK)         │     │ Id (PK)        │
│ StudentName    │     │ Name          │
│ StudentPhoto  │     │ Email         │
│ Message        │     │ Phone         │
│ CourseId (FK) │     │ Subject      │
│ CreatedAt      │     │ Message      │
│ IsActive       │     │ Status       │
│ DisplayOrder   │     │ CreatedAt    │
└─────────────────┘     └─────────────────┘

┌─────────────────┐
│   SiteSetting   │
├─────────────────┤
│ Key (PK)       │
│ Value_EN       │
│ Value_NP      │
│ Category      │
└─────────────────┘
```

### 2.2 Localization Strategy

| Approach | Implementation |
|----------|----------------|
| **Option A** | Separate `Title_EN`, `Title_NP` columns per entity |
| **Option B** | JSON column storing translations per entity |
| **Option C** | Resource files (.resx) for static strings |

**Recommended:** Option A + C hybrid - use database columns for content fields, resource files for UI labels

---

## 3. Project Structure

```
Consultancy/
├── Program.cs
├── appsettings.json
├── Consultancy.csproj
│
├── Data/
│   ├── AppDbContext.cs
│   └── Migrations/
│
├── Models/
│   ├── Entities/
│   │   ├── Course.cs
│   │   ├── Country.cs
│   │   ├── Category.cs
│   │   ├── Teacher.cs
│   │   ├── Student.cs
│   │   ├── Blog.cs
│   │   ├── Event.cs
│   │   ├── Testimonial.cs
│   │   ├── ContactInquiry.cs
│   │   └── SiteSetting.cs
│   └── ViewModels/
│       ├── CourseVM.cs
│       └── ...
│
├── Areas/
│   ├── Public/                    # Public-facing pages
│   │   ├── Pages/
│   │   │   ├── Index.cshtml
│   │   │   ├── About.cshtml
│   │   │   ├── Contact.cshtml
│   │   │   ├── Courses/
│   │   │   │   ├── Index.cshtml
│   │   │   │   └── Details.cshtml
│   │   │   ├── Countries/
│   │   │   │   ├── Index.cshtml
│   │   │   │   └── Details.cshtml
│   │   │   ├── Blogs/
│   │   │   │   ├── Index.cshtml
│   │   │   │   └── Details.cshtml
│   │   │   └── Events/
│   │   │       ├── Index.cshtml
│   │   │       └── Details.cshtml
│   │   └── _ViewStart.cshtml
│   │
│   └── Admin/                     # Admin panel
│       ├── Pages/
│       │   ├── Dashboard.cshtml
│       │   ├── Courses/
│       │   ├── Countries/
│       │   ├── Teachers/
│       │   ├── Students/
│       │   ├── Blogs/
│       │   ├── Events/
│       │   ├── Testimonials/
│       │   └── Inquiries/
│       └── _ViewStart.cshtml
│
├── Services/
│   ├── ICourseService.cs
│   ├── CourseService.cs
│   ├── ICountryService.cs
│   └── ...
│
├── TagHelpers/
│   └── LanguageTagHelper.cs
│
├── Components/
│   └── LanguageSelector.cshtml.cs
│
├── Shared/
│   ├── _Layout.cshtml
│   ├── _LayoutAdmin.cshtml
│   └── Components/
│       ├── Header/
│       ├── Footer/
│       └── Navbar/
│
├── Resources/
│   ├── Views/
│   └── wwwroot/
│       ├── css/
│       ├── js/
│       └── images/
│
└── wwwroot/
    ├── css/
    │   └── tailwind.css
    ├── js/
    └── images/
```

---

## 4. Page Routes

### Public Pages (`/{area?}/{page?}`)

| Route | Page | Description |
|-------|------|-------------|
| `/` | Home | Hero, featured courses, countries, testimonials, CTA |
| `/courses` | Course Index | All courses with filters |
| `/courses/{slug}` | Course Details | Single course details |
| `/countries` | Country Index | Study destinations |
| `/countries/{slug}` | Country Details | Single country info |
| `/teachers` | Teachers | Team page |
| `/blogs` | Blog Index | Latest news |
| `/blogs/{slug}` | Blog Details | Single blog post |
| `/events` | Event Index | Upcoming events |
| `/events/{slug}` | Event Details | Single event |
| `/about-us` | About | Company info |
| `/contact` | Contact | Contact form |

### Admin Pages (`/admin/{page}`)

| Route | Description |
|-------|-------------|
| `/admin` | Dashboard |
| `/admin/courses` | Manage courses |
| `/admin/countries` | Manage countries |
| `/admin/teachers` | Manage teachers |
| `/admin/students` | View registered students |
| `/admin/blogs` | Manage blogs |
| `/admin/events` | Manage events |
| `/admin/testimonials` | Manage testimonials |
| `/admin/inquiries` | View contact inquiries |
| `/admin/settings` | Site settings |

---

## 5. UI/UX Design (Tailwind)

### 5.1 Color Palette

```css
:root {
  --primary: #2563eb;      /* Blue-600 */
  --primary-dark: #1d4ed8;  /* Blue-700 */
  --secondary: #0f766e;   /* Teal-700 */
  --accent: #f59e0b;       /* Amber-500 */
  --dark: #1f2937;         /* Gray-800 */
  --light: #f3f4f6;       /* Gray-100 */
  --white: #ffffff;
}
```

### 5.2 Key Components

| Component | Tailwind Classes |
|-----------|------------------|
| Buttons | `px-6 py-2 rounded-lg bg-primary hover:bg-primary-dark text-white` |
| Cards | `bg-white rounded-xl shadow-md hover:shadow-lg transition` |
| Section | `py-16 bg-gray-50` |
| Container | `max-w-7xl mx-auto px-4 sm:px-6 lg:px-8` |
| Navbar | `fixed w-full bg-white/90 backdrop-blur shadow` |
| Footer | `bg-dark text-white py-12` |

### 5.3 Page Layouts

**Home Page Sections:**
1. Hero + CTA + Language Toggle
2. Featured Courses (Grid 3-col)
3. Countries (Grid 5-col)
4. Featured Teachers (Grid 4-col)
5. Testimonials (Slider)
6. Latest Blogs (Grid 3-col)
7. Call to Action Banner
8. Footer (Quick links, contact info, social)

---

## 6. Features to Implement

### 6.1 Core Features

- [ ] Multi-language (EN/NP) with URL switching
- [ ] Dynamic routing with slugs
- [ ] Course catalog with filtering
- [ ] Country study guides
- [ ] Teacher profiles
- [ ] Student registration form
- [ ] Contact inquiry form
- [ ] Blog/News system
- [ ] Events system
- [ ] Testimonials display
- [ ] Search functionality

### 6.2 Admin Features

- [ ] CRUD for all entities
- [ ] Image upload (stored in wwwroot/uploads/)
- [ ] Dashboard with statistics
- [ ] Inquiry management (mark read/resolved)
- [ ] Content moderation (publish/draft)
- [ ] Bulk actions

### 6.3 SEO Features

- [ ] Meta tags per page
- [ ] Sitemap generation
- [ ] Open Graph tags
- [ ] Clean URLs with slugs

---

## 7. Implementation Phases

### Phase 1: Foundation
1. Project setup + Tailwind config
2. Database context + migrations
3. Base layout + navigation
4. Language switching

### Phase 2: Core Pages
5. Home page
6. Course listing + details
7. Country listing + details
8. Teacher pages

### Phase 3: Content
9. Blog system
10. Event system
11. Testimonials
12. About + Contact pages

### Phase 4: Admin
13. Admin layout
14. Course/Country management
15. Teacher/Blog/Event management
16. Inquiry management

### Phase 5: Polish
17. Search functionality
18. SEO optimization
19. Performance tuning
20. Deployment

---

## 8. Quick Start Commands

```bash
# Create project
dotnet new webapp -o Consultancy

# Add EF Core
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Add Tailwind
npm init -y
npm install -D tailwindcss
npx tailwindcss init
```

---

## 9. Questions Before Implementation

1. **Branding:** Logo, primary brand colors (existing brand or new)?
2. **Hosting:** Where will you deploy (Azure, AWS, shared hosting)?
3. **Email:** How to send contact form notifications (SMTP, SendGrid)?
4. **Images:** Will admins upload images or use external URLs?
5. **Initial Data:** Need seeded sample courses/countries/teachers?