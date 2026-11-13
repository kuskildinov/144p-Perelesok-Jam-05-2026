# Ice Pieces WebGL Template — Unity 6

## Установка
1. Скопируйте папку `IcePieces` целиком в `<Unity Project>/Assets/WebGLTemplates/IcePieces/`.
2. В Unity откройте `Edit > Project Settings > Player > Web (WebGL) > Resolution and Presentation`.
3. Выберите `IcePieces` в поле `WebGL Template`.
4. Переключите платформу на Web/WebGL в Build Profiles и соберите проект.
5. Проверяйте сборку через локальный HTTP-сервер или хостинг, **не** открывайте `index.html` напрямую через `file://`.

## Файлы
- `index.html` — Unity 6 WebGL template и подключение Unity loader.
- `TemplateData/style.css` — оформление и адаптивность.
- `TemplateData/loading.js` — круговой прогресс, запуск, fullscreen, ошибки.
- `TemplateData/logo.svg` — временный фирменный знак в стилистике Ice Pieces; замените утверждённым логотипом студии.
- `TemplateData/favicon.svg` — иконка вкладки.

## Изменение оформления
Цвета в `TemplateData/style.css` в `:root`.
Замените `TemplateData/logo.svg` на собственный SVG с тем же именем или измените путь к логотипу в `index.html`.
Название игры подтягивается из Unity Player Settings через `PRODUCT_NAME`.

## Важно
- `createUnityInstance()` сообщает прогресс загрузки Unity, а не точный процент времени до готовности.
- После разрешения Promise Unity instance загрузчик плавно исчезает.
- На мобильных устройствах fullscreen зависит от возможностей браузера.
- Сжатые сборки Brotli/Gzip требуют корректных HTTP-заголовков и настройки хостинга; шаблон не исправляет серверную конфигурацию.
- Если после сборки Unity не распознала template-макросы, проверьте версию Unity и синтаксис макросов в её стандартном шаблоне.
