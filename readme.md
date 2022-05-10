## PUTS

Programavimo Uždavinių Tikrinimo Sistema

Programming Problems Testing System

A project that automates testing of simple C/C++ solutions to user created competitive programming problems. Main project is `PUTSWeb`.

## Docker image

This project uses Github Actions to automatically build a [docker image](https://hub.docker.com/repository/docker/benasbudrys/putsweb) for PUTSWeb.

## Deployment

Github Actions also deploy the [`rpi-stack.yml`](./Infrastructure/rpi-stack.yml) to a raspberry pi. It can be reached through [https://benaspi.ddns.net:8000](https://benaspi.ddns.net:8000)

## Local development

This project depends on a MySQL database. You could run one locally and connect it to this project with the `ConnectionString` in [`appsettings.json`](./PUTSWeb/appsettings.json).

An alternative to that would be to start up a MySQL docker container. For convenience there is a docker compose file for local development, run it with:

```
docker-compose -f Infrastructure/local-stack.yml up
```

The default `ConnectionString` should automatically connect PUTSWeb to the MySQL docker container.

---

Project created for gymnasium's maturity work by Benas Budrys, 2019

Infrastructure added later in 2022