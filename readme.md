# Подключение стика (для Windows)

[Источник](https://learn.microsoft.com/en-us/windows/wsl/connect-usb#attach-a-usb-device)

Получение списка USB устройств:
``` sh
usbipd list 
# ...
# 3-3    10c4:ea60  LibUSB-1.0: BrailleMemo [Pocket], Seika [Braille Display]     Not shared
# ...
```

Привязка устройства к WSL через bus id (выполняется один раз):
```
usbipd bind --busid 3-3 
```

Прикрепление устройства к WSL (выполняется при каждом переподключении стика):
``` sh
usbipd attach --wsl --busid 3-3 
```

``` sh
usbipd list
# ...
# 3-3    10c4:ea60  LibUSB-1.0: BrailleMemo [Pocket], Seika [Braille Display]     Attached
# ...
```

Переключение на WSL и проверка подключения устройства:
``` sh
wsl
ls /dev/ttyUSB*
# /dev/ttyUSB0
```

Если устройство не было прикреплено, нужно вручную включить драйвера в WSL и выполнить снова подключение стика:
``` sh
modprobe usbserial
modprobe cp210x
```

# Развертывание контейнеров 

Привязка стика к контейнеру `compose.yaml`: 
``` yml
homeassistant: 
    # ...
    devices: 
        - /dev/ttyUSB0:/dev/ttyUSB0
    # ...
```

Развертывание и настройка контейнера Home Assistant (HA):
``` sh
docker compose up -d homeassistant 
```

[Получение ключа](https://developers.home-assistant.io/docs/api/rest/) HA REST API. 

Запись ключа в переменную окружения сервиса Plug Control (PC):
``` yml 
plugcontrolservice:
    # ...
    environment: 
        HA_KEY: "<your-key>"
    # ...
```

Развертывание сервиса PC:
``` sh
docker compose up -d
```

Доп. информация и источники: 
- [Пример compose-файла](https://www.home-assistant.io/installation/linux)
- [Доки Aqara Smart Plug](https://object.pscloud.io/cms/cms/Uploads/file_0_3890_10_0_0_8HPF4V.pdf)
- [Доки Jethome JetStick](https://docs.jethome.ru/ru/zigbee/sticks/jetstick_z2.html)
- [Подключение розетки через стик через интеграцию ZHA](https://docs.jethome.ru/ru/controllers/linux/howto/homeassistant/zha.html)

# Тестирование сервиса 

Entity Id можно найти в списке сущностей (entities) устройства в Home Assistant. 

Включение розетки:
``` sh
curl -X POST "http://localhost:35000/api/plug/set-state" \
    -H "Content-Type: application/json" \
    -H "Host: localhost" \
    -d '{ "entityId": "switch.plug_switch", "state": "on" }'
```

Выключение розетки:
``` sh
curl -X POST "http://localhost:35000/api/plug/set-state" \
    -H "Content-Type: application/json" \
    -H "Host: localhost" \
    -d '{ "entityId": "switch.plug_switch", "state": "off" }'
```
