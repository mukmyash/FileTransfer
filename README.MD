# File Transfer
Приложение занимается прослушкой каталога и с появлением в нем файла выполняет запрограммированные действия.

## Конфигурация
#### Секция FileScannerHostedService
- BackupPath - Каталог для бэкапа файла. Если не указан, то ни чего бэкапироваться не будет.
- ScanPeriodSeconds - Периодичность проверки каталога на наличие нового файла.
- FileProviderType - Тип файлового провайдера
  - SMB - файлова шара Windows. (SAMBA)
  - _В перспективе FTP и локальный каталог_
- FileProviderSettings - Настройки для файлового провайдера. Для каждого типа провайдера свои настройки.
- WatchPath - Путь который будет прослушиваться 
    - *Если FileProviderType = SMB*. Указывается путь без IP.
    Например: </br>
        Оригинальный путь: _smb://192.168.1.1/_**path/to/folder**</br>
        Указывается только **path/to/folder**.
- Steps - Операции которые необходимо выполнить над файлом. (Список поддерживаемых операций представлен ниже.)
    - Type - тип операции
    - Settings - настройки операции

#### FileProviderSettings
##### SMB
- ServerIP - IP адрес сервера SAMBA (Например 192.168.1.1)
- Login - логин для подключения к шаре
- Password - пароль для подключения к шаре.

#### Steps
##### MapWhenFileName
Проверяет имя файла по регулярному выражению и если совпадение найдено то пускает файл по альтернативному маршруту.

**Настройки**
- Mask - Регулярное выражение для проверки имени файла
- Steps - Последовательность операций в случае совпадений маски файла

##### MapWhenException
Данный шаг отлавливает ошибки и отправляет файл по другому маршруту.

**Настройки**
- Steps - Последовательность операций в случае возникновения ошибки

##### FileNameTransformation
Преобразует имя файла в соответствии с маской.

  **Настройки**
  - FileMask - Маска файла
  - ParametersDescription - массив с описание параметров для подстановки.

**Описание параметров**

*XMLContent*

Настройки

``` JSON
  {
    // Имя параметра (каждый извлеченный параметр будет иметь имя FPi, где i - порядковый номер параметра)
    "ParameterName": "FP",

    // Тип файла из которого будут извлекаться параметры
    "FileType": "Input",

    // Дефолтное значение для параметра
    "DefaultValue": "0",

    // Тип параметра (Всегда XMLContent для данного типа)
    "ParameterType": "XMLContent",

    // Путь до XML элемента или атрибута.
    "XPath": "/xpath/to/element/or/attribute"
  }
```

Извлекает из содержимого XML документа значения узлов или атрибутов по указанному XPath. Извлекает ровно один параметр и присваивает ему имя указонное в секции _ParameterName_.

*_FileName_*

Настройки

``` JSON
  {
    // Имя параметра (каждый извлеченный параметр будет иметь имя FPi, где i - порядковый номер параметра)
    "ParameterName": "FP",

    // Тип файла из которого будут извлекаться параметры
    "FileType": "Input",

    // Дефолтное значение для параметра
    "DefaultValue": "0",

    // Тип параметра (Всегда FileName для данного типа)
    "ParameterType": "FileName",

    // Список разделителей
    "Separators": [ "_", "-" ]
  }
```

 Исходное имя файла разбивается на параметры по указанным спецсимволам. Каждому параметру присваивается имя из параметра _ParameterName_, которое можно использовать в маске. 
 
Например

Конфигурация
``` JSON
 {
    "ParameterName": "FP",
    "FileType": "Input",
    "DefaultValue": "0",
    "ParameterType": "FileName",
    "Separators": [ "_", "-" ]
  }
```
Имя файла: параметр1_параметр2-параметр3_-параметр5

Из данного файла получатся следующие параметры:
- **@{FP1}** - "параметр1"
- **@{FP2}** - "параметр2"
- **@{FP3}** - "параметр3"
- **@{FP4}** - ""
- **@{FP5}** - "параметр5"

Маска файла: **@{FP1}-текст-@{FP5}__--@{FP3}_@{fp4}.ttxxtt**

Результат: параметр1-текст-параметр5__--параметр3_@{fp4}.ttxxtt


##### XSDValidation
Проверяет файл по XSD схеме. Если файл не удовлетворяет XSD схеме генерируется исключение и файл откатывается до ближайшего шага с обработкой исключения 

**Настройки**
- XSDPath - Путь до XSD схемы

##### XSLTransformation
Применяет к файлу XSLT преобразование (На текущий момент поддерживается XSLT 1.0 и XPath 1.0)

**Настройки**
- XSLTPath - Путь до XSLT преобразования

##### Export
Экспортирует файл в указаное место

**Настройки**
- FileType - Файл который нужно экспортировать: 
    - Input - Оригинальный входной файл
    - Output - Файл после применения всех преобразований
    - Exception - Экспортирует информацию об ошибке
- FileProviderType - тип поставщика файлов 
- FileProviderSettings - Настройки поставщика файлов
- Path - путь для экспорта


# Пример конфигурации

``` JSON
{
  "Logging": {
    "Console": {
      "IncludeScopes": true,
      "LogLevel": {
        "Default": "Trace"
      }
    },
    "LogLevel": {
      "Default": "Trace"
    }
  },
  "FileScannerHostedService": {
    "BackupPath": "./backup/files",
    "WatchPath": "Elements SE/ftp/Start",
    "ScanPeriodSeconds": 5,
    "FileProviderType": "SMB",
    "FileProviderSettings": {
      "ServerIP": "192.168.1.1",
      "Login": "admin",
      "Password": "admin"
    }
  },
  "Steps": [
    {
      "Type": "MapWhenException",
      "Settings": {
        "Steps": [
          {
            "Type": "Export",
            "Settings": {
              "FileType": "Input",
              "Path": "Elements SE/ftp/Error/",
              "FileProviderType": "SMB",
              "FileProviderSettings": {
                "ServerIP": "192.168.1.1",
                "Login": "admin",
                "Password": "admin"
              }
            }
          },
          {
            "Type": "Export",
            "Settings": {
              "FileType": "Exception",
              "Path": "Elements SE/ftp/Error/",
              "FileProviderType": "SMB",
              "FileProviderSettings": {
                "ServerIP": "192.168.1.1",
                "Login": "admin",
                "Password": "admin"
              }
            }
          }
        ]
      }
    },
    {
      "Type": "MapWhenFileName",
      "Settings": {
        "Mask": "\\d*.xml",
        "Steps": [
          {
            "Type": "XSDValidation",
            "Settings": {
              "XSDPath": "./XSD/test.xsd"
            }
          },
          {
            "Type": "XSLTransformation",
            "Settings": {
              "XSLTPath": "./XSLT/test.xslt"
            }
          },
          {
            "Type": "Export",
            "Settings": {
              "FileType": "Output",
              "Path": "Elements SE/ftp/Success/",
              "FileProviderType": "SMB",
              "FileProviderSettings": {
                "ServerIP": "192.168.1.1",
                "Login": "admin",
                "Password": "admin"
              }
            }
          }

        ]
      }
    },
    {
      "Type": "MapWhenFileName",
      "Settings": {
        "Mask": "\\w*.txt",
        "Steps": [
          {
            "Type": "FileNameTransformation",
            "Settings":  {
              "FileMask": "@{FP1}_@{FP2}.txt",
              "ParametersDescription": [
                {
                  "ParameterName": "FP",
                  "FileType": "Input",
                  "DefaultValue": "0",
                  "ParameterType": "FileName",
                  "Separators": [ "_", "-" ]
                },
                {
                  "ParameterName": "XML",
                  "FileType": "Output",
                  "DefaultValue": "VALUE",
                  "ParameterType": "XMLContent",
                  "XPath": "/xpath/to/element/or/attribute"
                }
              ]
            }
          },
          {
            "Type": "Export",
            "Settings": {
              "FileType": "Output",
              "Path": "Elements SE/ftp/Success/",
              "FileProviderType": "SMB",
              "FileProviderSettings": {
                "ServerIP": "192.168.1.1",
                "Login": "admin",
                "Password": "admin"
              }
            }
          }
        ]
      }
    },
    {
      "Type": "Export",
      "Settings": {
        "FileType": "Input",
        "Path": "Elements SE/ftp/ErrorMapping/",
        "FileProviderType": "SMB",
        "FileProviderSettings": {
          "ServerIP": "192.168.1.1",
          "Login": "admin",
          "Password": "admin"
        }
      }
    }
  ]
}
```
