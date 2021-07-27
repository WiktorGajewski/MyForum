# MyForum

### Target
Create simple ASP.NET Core web app: **Internet forum**. 
Using Razor Pages.

#### Features
- Users, every user has one of 3 Ranks:
  - Leader (a.k.a. Admin) - managing users, changing ranks
  - Guildmaster (a.k.a. manager of a group) - managing his own guild, inviting and removing members
  - Novice (a.k.a. normal user)
- Guilds (a.k.a. Groups) - every Guild has it's own Guildmaster
- Authentication using ASP.NET Core Identity (local accounts)
- Authorization (based on Ranks)
- Captcha challenge from Goggle during registration <br>
  (and an option to turn it on or off in appsettings.json)
- MainForum - open for every authorized user
- Forum for every Guild - only for members
- Likes
- Prestige Points (every user gets a point for every like he manages to get)
<br>

### Configuration options
#### in appsettings.json
path: MyForum/appsettings.json<br>
1. Default Rank for new users.<br>
Property: "MySettings:DefaultRank"<br>
Possible values: "Novice", "Guildmaster", "Leader"<br>
Default: "Novice"<br>

2. Enable/Disable Captcha<br>
Property: "MySettings:CaptchaOn" (true/false)<br>
Default: false<br>
In order to enable catpcha you need to also set Google reCaptcha Site Key and Secret Key.<br>
Site key: "GoogleReCaptcha:key"<br>
Secret key: "GoogleReCaptcha:secret"<br>
