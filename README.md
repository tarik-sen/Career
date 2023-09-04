# Career
Career is a web platform built using C# 11 and ASP.NET Core 7 that aims to provide a comprehensive job-seeking and recruitment solution, similar to popular job platform kariyer.net.<br/>
It is written only for educational purposes and not suitable for any kind of organization in its current state.

> ⚠️ Current version supports only one Company account!

## Features for recruiters
- Create, Review, Update and Delete Job Posts
- Visualization of candidate data with charts.
- Notification system for job recruitment
- Various ordering options during candidate selection.
- View candidate resumes.

## Features for job seekers
- Build your detailed CV with a vast amount of features from contact info to any kind of skill
- Filter job posts to suit your needs.
- Apply a job and get notified when recruiter wants you in.
- Account managment

## Requirements
  - Visual Studio with C# & asp.net core
  - Microsoft SQL Server

## Installation
Clone the repository and open it via Visual Studio
```bash
git clone https://github.com/tarik-sen/Career.git
```

> ⚠️ If dotnet-ef is not installed, run "dotnet tool install --global dotnet-ef".

Create a terminal and run these commands (change the <pw> with a proper - strong - password) to create the database and set user & admin account passwords.
```bash
dotnet ef database update
dotnet user-secrets set UserPW <PW>
dotnet user-secrets set AdminPW <PW>
```

That's all, run the application and enjoy your time!

## Contributing
Contributions are welcome! If you find a bug or have an enhancement in mind, feel free to open an issue or submit a pull request.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Sample Images
<img src=docs/images/demo-1.png>
<p align="center"><em>Screenshot</em>: A preview of the Career platform's home page.</p>
<br/>

<img src=docs/images/demo-2.png>
<p align="center"><em>Screenshot</em>: A preview of the Career platform's job openings page.</p>
<br/>

<img src=docs/images/demo-3.png>
<p align="center"><em>Screenshot</em>: A preview of the Career platform's log in page.</p>
<br/>

<img src=docs/images/demo-4.png>
<p align="center"><em>Screenshot</em>: A preview of the Career platform's job application page.</p>
<br/>

<img src=docs/images/demo-5.png>
<p align="center"><em>Screenshot</em>: A preview of the Career platform's account managment page.</p>
<br/>

<img src=docs/images/demo-6.png>
<p align="center"><em>Screenshot</em>: A preview of the Career platform's recruiter hire page.</p>
<br/>

<img src=docs/images/demo-7.png>
<p align="center"><em>Screenshot</em>: A preview of the Career platform's recruiter applicant resume view page.</p>
<br/>

<img src=docs/images/demo-8.png>
<p align="center"><em>Screenshot</em>: A preview of the Career platform's recruiter job CRUD operations page.</p>
<br/>

