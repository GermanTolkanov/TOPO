# Памятка по работе с гитом
## Установка гит на Windows
Для начала работы с гитом на ОС Windows нужно перейти по ссылке и скачать установочный файл <https://git-scm.com/download/win>  
## Создание репозитория
git init - Cоздать локальный репозиторий  
## Настройка гита
git config --global user.name\user.email - Привязать коммиты к имени\почте, флаг global нужен для кэширования учётных данных  
git init - Создать или инициализировать существующий репозиторий в текущей директории  
## Коммит версий
git status - Узнать если какие-либо неотслеживаемые или подготовленные файлы для комита  
git add <название файла> - Добавить файл в область подготовленных  
git add . - Добавть все файлы в область подготовленных
git commit -m "коментарий" - Коммит добавленных файлов с коментарием  
git log - Просмотр истории комитов в обратном порядке  
git diff - Просмотр разницы между комитами...
## Работа с ветками
git branch <Ветка> - Поздание новой ветки  
git checkout (-b) <Ветка> - Переход на ветку; ключ -b означает создание новой ветки  
git branch - Просмотр списка веток  
git branch -d <Ветка> - Удалить ветку  
git merge <Ветка> - Объединение главной и указанной веток  
## Удаленный репозиторий 
git remote add <remote name> <URL> - Указать адрес сервера в репозитории  
git push - Запушить основную ветку в удаленный репозиторий  
git pull - Спулить данные с удаленного репозитория  
## Слияние

### Конфликты
Выполнение команды слияния прерывается в самом начале, если Git обнаруживает изменения в рабочем каталоге или разделе проиндексированных файлов текущего проекта. Git не выполнит слияние.  
Конфликт между текущей локальной веткой и веткой, с которой выполняется слияние. Git попытается объеденить версии, но останутся ошибки, которые придётся изменять вручную.