# Лабораторная работа №7
## CI/CD

CI/CD - это комбинация непрерывной интеграции (continuous integration) и непрерывного развертывания (continuous delivery или continuous deployment) программного обеспечения в процессе разработки

### Задание:
Насторить автоматическое развертывание своего приложения(как мининимум серверной части) на кластере ВолгГТУ, для этого:
1. Создать файл в своем репозитории с расширением yml по пути `./.github/workflows`
2. Наполнить файл для выполнения 2-х jobs: запуск модульных тестов и разворачивание на кластере
3. Сделать проброс портов к кластеру с помощью ssh тунеля
4. Настроить браузер, для получения доступа к приложению

### Шаги:
Пример скрипта для настройки CI/CD находится по пути `.github/workflows/build-dotnet-and-deploy.yml`.

В нем описывается когда нужно запускать скрипт. В данном случае при push в ветку feature/Lab7
```
push:
    branches: ["feature/Lab7"]
```

Также в нем присутсвуют 2 jobs: `test` и `deploy`. Которые запускают модульные тесты и разворачивание на сервере соответсвенно. В зависимости от используемой технологии и языка скрипт может отличаться. Для своего прилоджения необходимо найти пример в открытых источниках.

Разворачивание на сервер происходит одним из простых способов - с помощью ssh подключения к серверу, для этого:
1. Указываем среду github в которой будет выполняться скрипт
```
runs-on: ubuntu-latest
```
2. Указываем action(готовая библиотека от сторонних разработчиков), который хотим использовать
```
uses: appleboy/ssh-action@v0.1.10
```
3. Для подключения к кластеру необходимо указать данные 
```
host: "cluster.vstu.ru:57322"
username: ${{ secrets.USERNAME }}
password: ${{ secrets.PASSWORD }}
```

Для аутентификации на кластере можно использовать одного из 8 пользователей:
- login: user0-user7
- password: userIntelCuda0-userIntelCuda7

> Чтобы не светить свой пароль в открытых источниках, можно поместить его в раздел github secrets (Settings -> Secrets and variables -> Actions -> New repository secret).

4. Наполнить инструкции для деплоя при подключении по ssh. В данном примере через git затягиваются последнии изменения, останавливается и удаляется `podman` контейнер, удаляется образ и создается заново
```
script: |
          cd weather-app
          git checkout feature/Lab7
          git pull
          podman stop weather_backend
          podman rm weather_backend
          podman rmi weather_backend_image
          podman build -f WeatherBackend/Dockerfile -t weather_backend_image .
          podman run -p 64001:8080 --name weather_backend -d weather_backend_image
```

5. Проверить успешность выполнения `jobs` во вкладке `Action`

##### Т.к. открытие портов у кластера наружу запрещено, для доступа к своему приложению будем использовать ssh тунель, для этого [Клик](https://winitpro.ru/index.php/2019/10/29/windows-ssh-tunneling/):

1. В оболочке Windows powershell установите утилиту ssh.
`Get-WindowsCapability -Online | ? Name -like 'OpenSSH.Client*'`

2. Выполните вход на кластер
`ssh -L <local_port>:127.0.0.1:<cluster_port> <user_login>@79.170.167.30 -p 57322`

> После аутентификации, порт с кластера(<cluster_port>) будет прокинут на локальный порт(<local_port>)

3. В браузере Mozilla Firefox настроить сеть [Клик](https://bash-prompt.net/guides/ssh-firefox-tunnel/):

`Настройки -> Настройки сети -> Ручная настройка прокси -> Узел SOCKS: 79.170.167.30, Порт: <cluster_port> -> SOCKS 5 -> OK`

4. В браузере проверить доступ к приложению по адресу `127.0.0.1:<local_port>`