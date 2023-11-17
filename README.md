# Hierarchical structure of directories
## Terms of reference for the .Net course from GFL.

### Technologies used
The project was implemented using ASP.NET Web app MVC. Also, JS (AJAX) was used to send the path of directories of the OS to the backend and send JSON with the directory structure.

### Creating a database
To fulfil the task, I used the MySQL database. The creation of the database and the user for the work was performed by the script below
```
  CREATE DATABASE IF NOT EXISTS HierarchicalDirectoriesDB;

  CREATE USER IF NOT EXISTS 'entity_framework'@'localhost' IDENTIFIED BY 'S8cr8tP4ssw0rd369';
  
  GRANT ALL PRIVILEGES ON HierarchicalDirectoriesDB.* TO 'entity_framework'@'localhost';
```

The table for the directories was created using the Entity Framework (migrations are available in the repository).

### First bonus task
Implemented the logic of parsing the directory path from on the backend and saving the structure of this directory to the database. But I failed to implement the ability to import the directory structure from the OS on frontend. So, I tried to do this by having the user select a folder on the frontend and send the path of it to the backend, but all the methods I tried only gave me the name of the selected directory. I'm not sure if this is possible on the Web, or maybe I misunderstood the task.

As for importing the structure from a file, I did it by implementing reading the directory structure from JSON. The file for testing the import of the directory structure from JSON is available in the repository (`FileForTesingImportOfJSON.json`).
