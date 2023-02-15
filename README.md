# SystemMonitor
SystemMonitor is a system for remote monitoring hardware usage of Linux and Windows machines.

## Tech Stack
- **Client:** ReactJS, MaterialUI
- **Server:** .NET 6, ASP.NET Core, EF Core, SignalR
- **Agent:** .NET 6 (it's a console app, waddaya need?)
- **Database:**: MS SQL

Database, Server and WebApp are deployed using Docker
## Docker images
Docker images are publicly available on docker hub:
- WebAPI: https://hub.docker.com/r/marchewjapko/system-monitor-api
- WebApp: https://hub.docker.com/r/marchewjapko/system-monitor-web-app

## Screenshots
![App Screenshot](/Helpers/dashboard-light-vs-dark.png?raw=true)
![App Screenshot](/Helpers/details-light-vs-dark.png?raw=true)

## Deployment
To deploy this project download `docker-compose.yml`, the rest of the repo is not necessary unless you want to compile it yourself. Then run the following command on it:

```bash
  docker-compose up
```

Next configure Agent's `appsettings.json` by setting the address of your server. After that simply run the agent app.

## Disclaimer
It swallows RAM, a lot of RAM.
