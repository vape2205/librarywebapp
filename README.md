<h1 align="center" id="title">WEBAPP LIBRARY</h1>

<p align="center"><img src="https://github.com/vape2205/librarywebapp" alt="project-image"></p>

<p id="description">APLICACIÓN WEB DE LIBRERIA</p>

  
  
<h2>🧐 Features</h2>

Caracteristicas

*   Crear suscripciones
*   Activar suscripciones
*   Cancelar suscripciones
*   Listado de libros
*   Lectura de libros

<h2>🛠️ Installation Steps:</h2>

<p>1. Agregar archivo de environment</p>

Crear un archivo .env en la carpeta donde se encuentre el archivo docker-compose

```
# Variables de entorno para la bd 
URL_APIAUTH=http://auth.api:5000
URL_APILIBRARY=http://library.api:6000
URL_APISUSCRIPTIONS=http://suscriptions.api:6001
WEBAPP_PORT=6002
```

<p>2. Ejecutar Docker Compose</p>

```
docker compose up -d --build
```

<h2>💻 Built with</h2>

Tecnologias usadas en este proyecto:

*   ASP .NET 8