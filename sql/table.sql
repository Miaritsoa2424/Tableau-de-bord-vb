DROP DATABASE IF EXISTS voiture_db;
CREATE DATABASE IF NOT EXISTS voiture_db;
USE voiture_db;

CREATE TABLE IF NOT EXISTS voiture (
    id INT AUTO_INCREMENT PRIMARY KEY,
    marque VARCHAR(100) NOT NULL,
    acceleration DOUBLE NOT NULL,
    deceleration DOUBLE NOT NULL,
    consommation DOUBLE NOT NULL,
    carburant DOUBLE NOT NULL,
    carburant_max DOUBLE NOT NULL,
    v_max DOUBLE NOT NULL
);
CREATE TABLE IF NOT EXISTS evenement (
    id_event INT AUTO_INCREMENT PRIMARY KEY,
    id_voiture INT NOT NULL,
    acceleration DOUBLE NOT NULL,
    v_init DOUBLE NOT NULL,
    date DATETIME NOT NULL,
    FOREIGN KEY (id_voiture) REFERENCES voiture(id)
);

