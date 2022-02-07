DROP DATABASE IF EXISTS velomax;
CREATE DATABASE velomax;
USE velomax;
CREATE USER if not exists 'bozo'@'localhost' IDENTIFIED BY 'bozo';
grant select on velomax.* to 'bozo'@'localhost';

CREATE TABLE Velo(
	numProduit VARCHAR(4) NOT NULL,
    nom VARCHAR(20) NOT NULL,
    grandeur VARCHAR(20) NOT NULL,
    prix INT,
    `type` VARCHAR(20),
    dateIntro DATE NOT NULL,
    dateDiscont DATE NULL,
    nbcommande INT DEFAULT 0,
    PRIMARY KEY (numProduit) 
);
    
CREATE TABLE Assemblage(
	numVelo VARCHAR(4) NOT NULL,
    cadre VARCHAR(5) NOT NULL,
    guidon VARCHAR(5) NOT NULL,
    frein VARCHAR(5) NULL,
    selle VARCHAR(5) NOT NULL,
    derailleurAvant VARCHAR(5) NULL,
    derailleurArriere VARCHAR(5) NULL,
    roueAvant VARCHAR(5) NOT NULL,
    roueArriere VARCHAR(5) NOT NULL,
    reflecteurs VARCHAR(5) NULL,
    pedalier VARCHAR(5) NOT NULL,
    ordinateur VARCHAR(5) NULL,
    panier VARCHAR(5) NULL,
    PRIMARY KEY (numVelo),
    FOREIGN KEY(numVelo) REFERENCES velo(numProduit)
    ON DELETE CASCADE
);

CREATE TABLE  Fournisseur (
	Siret VARCHAR(10) NOT NULL,
	nom_Entreprise VARCHAR(20) NOT NULL,
	nom_Contact varchar(20) NOT NULL,
	Adresse VARCHAR(50) NOT NULL,
	Label INT,
    PRIMARY KEY(Siret)
);

CREATE TABLE Pieces(
	numPiece VARCHAR(5) NOT NULL,
    descr VARCHAR(40) NOT NULL,
    Siret VARCHAR(10) NOT NULL,
    num_Produit_fournisseur VARCHAR(5) NOT NULL,
    prix INT NOT NULL,
    dateIntro DATE NOT NULL,
    dateDiscont DATE NULL,
    delaiAppr INT NOT NULL,
    quantite INT,
    nbcommande INT DEFAULT 0,
    PRIMARY KEY(numPiece, Siret),
    foreign key(Siret) REFERENCES Fournisseur(Siret)
);

CREATE TABLE PieceFournisseur(
	Siret VARCHAR(10) NOT NULL,
    numProduit VARCHAR(5) NOT NULL,
    PRIMARY KEY(numProduit, Siret),
    FOREIGN KEY(Siret) REFERENCES Fournisseur(Siret)
);

CREATE TABLE Clients (No_client INT PRIMARY KEY NOT NULL, 
	 nom varchar(20), prenom VARCHAR(20), nom_compagnie varchar(20),
	 adresse VARCHAR(100) NOT NULL, mail varchar(100) NOT NULL,
	 telephone int NOT NULL, nom_contact varchar(20), adhesion bool
);

CREATE TABLE  COMMANDE (
	No_commande	VARCHAR(100)  PRIMARY KEY,
	No_client INT NOT NULL,
	date_commande DATE NOT NULL,
	adresse_livraison CHAR(40)  NOT NULL,
	date_livraison DATE,
	CONSTRAINT  No_client  FOREIGN KEY  (No_client)  REFERENCES  CLIENTS(No_client)
    ON DELETE CASCADE
);

DELIMITER |
CREATE TRIGGER before_insert_commande
BEFORE INSERT
ON Commande
FOR EACH ROW
BEGIN
	IF new.date_livraison IS NULL OR new.date_livraison = "0001-01-01" THEN
    SET new.date_livraison = DATE_ADD(new.date_commande, INTERVAL 1 year);
	END IF;
END |
DELIMITER ;

CREATE TABLE  ITEMCOMMANDE (
	No_commande	VARCHAR(100),
	Ref_item VARCHAR(10), 
	quantite_item	INT  NOT NULL,
	PRIMARY KEY (No_commande, Ref_item),
	CONSTRAINT  No_commande  FOREIGN KEY  (No_commande)  REFERENCES  COMMANDE(No_commande)
    ON DELETE CASCADE
);

DELIMITER |
CREATE PROCEDURE decrementation_piece
(IN Ref_item VARCHAR(5), quantite_item INT)
BEGIN
UPDATE Pieces
SET Pieces.quantite  = Pieces.quantite  - quantite_item, Pieces.nbcommande = Pieces.nbcommande + quantite_item
WHERE Ref_item = Pieces.numPiece AND Pieces.quantite>0 LIMIT 1;
END |

CREATE TRIGGER before_insert_itemcommande
BEFORE INSERT
ON ITEMCOMMANDE
FOR EACH ROW
BEGIN
	IF REGEXP_LIKE(new.Ref_item, '^-?[0-9]+$') THEN
	SELECT cadre, guidon, frein, selle, derailleurAvant, derailleurArriere, reflecteurs, pedalier, ordinateur, panier 
    INTO @cadre, @guidon, @frein, @selle, @derailleurAvant, @derailleurArriere, @reflecteurs, @pedalier, @ordinateur, @panier 
    FROM assemblage WHERE Assemblage.numVelo = new.Ref_item;
    CALL decrementation_piece(@cadre, new.quantite_item);
    CALL decrementation_piece(@guidon, new.quantite_item);
    CALL decrementation_piece(@frein, new.quantite_item);
    CALL decrementation_piece(@selle, new.quantite_item);
    CALL decrementation_piece(@derailleurAvant, new.quantite_item);
    CALL decrementation_piece(@derailleurArriere, new.quantite_item);
    CALL decrementation_piece(@reflecteurs, new.quantite_item);
    CALL decrementation_piece(@pedalier, new.quantite_item);
    CALL decrementation_piece(@ordinateur, new.quantite_item);
    CALL decrementation_piece(@panier, new.quantite_item);
    UPDATE Velo SET nbcommande = nbcommande + 1 WHERE numProduit = new.Ref_item;
    ELSE
    UPDATE PIECES SET quantite=quantite-new.quantite_item, nbcommande=nbcommande + new.quantite_item WHERE pieces.numPiece = new.Ref_item AND quantite>=new.quantite_item LIMIT 1;
	END IF;
END |
DELIMITER ;

CREATE TABLE  Car_Fidelio (
	No_programme INT PRIMARY KEY NOT NULL, descriptions VARCHAR(20) not null, cout int NOT NULL, duree VARCHAR(6) not null,
	rabais INT NOT NULL
);
CREATE TABLE  Fidelio (
	No_client INT not null, No_programme int not null,date_inscription DATE, 
	FOREIGN KEY (No_client) REFERENCES Clients (No_client)
    ON DELETE CASCADE
    , FOREIGN KEY (No_programme) REFERENCES Car_Fidelio (No_programme)
    
);

INSERT INTO velo VALUES ('101', 'Kilimandjaro', 'Adultes', '569', 'VTT', '2000-07-31', NULL, 0); 
INSERT INTO velo VALUES ('102', 'NorthPole', 'Adultes', '329', 'VTT', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('103', 'MontBlanc', 'Jeunes', '399', 'VTT', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('104', 'Hooligan', 'Jeunes', '199', 'VTT', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('105', 'Orléans', 'Hommes', '229', 'Vélo de course', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('106', 'Orléans', 'Dames', '229', 'Vélo de course', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('107', 'BlueJay', 'Hommes', '349', 'Vélo de course', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('108', 'BlueJay', 'Dames', '349', 'Vélo de course', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('109', 'Trail Explorer', 'Filles', '129', 'Classique', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('110', 'Trail Explorer', 'Garçons', '129', 'Classique', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('111', 'Night Hawk', 'Jeunes', '189', 'Classique', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('112', 'Tierra Verde', 'Hommes', '199', 'Classique', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('113', 'Tierra Verde', 'Dames', '199', 'Classique', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('114', 'Mud Zinger I', 'Jeunes', '279', 'BMX', '2000-07-31', NULL,0); 
INSERT INTO velo VALUES ('115', 'Mud Zinger II', 'Adultes', '359', 'BMX', '2000-07-31', NULL,0); 

INSERT INTO Fournisseur VALUES ('446634983', 'Lucien', 'Abdel Koichi','12 Rue Champs-Elysée 75001 Paris', 1);
INSERT INTO Fournisseur VALUES ('53116516', 'Marcel', 'Abdel Koichi','12 Rue Champs-Elysée 75001 Paris', 2);
INSERT INTO Fournisseur VALUES ('87740679', 'Jules', 'Abdel Koichi','12 Rue Champs-Elysée 75001 Paris', 2);
INSERT INTO Fournisseur VALUES ('87402867', 'Georges', 'Abdel Koichi','12 Rue Champs-Elysée 75001 Paris', 2);
INSERT INTO Fournisseur VALUES ('66800362', 'Fred', 'Abdel Koichi','12 Rue Champs-Elysée 75001 Paris', 3);
INSERT INTO Fournisseur VALUES ('48899036', 'Germain', 'Abdel Koichi','12 Rue Champs-Elysée 75001 Paris', 3);
INSERT INTO Fournisseur VALUES ('69179980', 'Antoine', 'Abdel Koichi','12 Rue Champs-Elysée 75001 Paris', 3);
INSERT INTO Fournisseur VALUES ('72017119', 'Fernand', 'Abdel Koichi','12 Rue Champs-Elysée 75001 Paris', 3);
INSERT INTO Fournisseur VALUES ('08575264', 'Emile', 'Abdel Koichi','12 Rue Champs-Elysée 75001 Paris', 1);
INSERT INTO Fournisseur VALUES ('70157979', 'Marcel','Abdel Koichi', '12 Rue Champs-Elysée 75001 Paris', 4);

INSERT INTO pieces VALUES('C32', 'Cadre', '446634983', 'C1', '12', '2000-07-31', NULL, 2, 3, 0);
INSERT INTO pieces VALUES('G7', 'Guidon', '446634983', 'G1', '15', '2000-07-31', NULL, 2, 6, 0);
INSERT INTO pieces VALUES('C32', 'Cadre', '53116516', 'C6', '13', '2000-07-31', NULL, 2, 0, 0);
INSERT INTO pieces VALUES('F3', 'Freins', '53116516', 'F3', '9', '2000-07-31', NULL, 2, 10, 0);
INSERT INTO pieces VALUES('S35', 'Selle', '48899036', 'S8', '14', '2000-07-31', NULL, 2, 5, 0);
INSERT INTO pieces VALUES('P12', 'Pédalier', '70157979', 'P9', '17', '2000-07-31', NULL, 2, 4, 0);
INSERT INTO pieces VALUES('C34', 'Cadre', '69179980', 'C12', '25', '2000-07-31', NULL, 2, 0, 0);
INSERT INTO pieces VALUES('C15', 'Cadre', '48899036', 'C11', '25', '2000-07-31', NULL, 2, 4, 0);
INSERT INTO pieces VALUES('C43', 'Cadre', '66800362', 'C4', '20', '2000-07-31', NULL, 2, 0, 0);

INSERT INTO pieces VALUES('C76', 'Cadre', '87740679', 'C7', '20', '2000-07-31', NULL, 2, 5, 0);
INSERT INTO pieces VALUES('C44f', 'Cadre', '66800362', 'C41', '22', '2000-07-31', NULL, 2, 2, 0);
INSERT INTO pieces VALUES('C43f', 'Cadre', '66800362', 'C4f', '22', '2000-07-31', NULL, 2, 6, 0);
INSERT INTO pieces VALUES('C01', 'Cadre', '87740679', 'C01', '17', '2000-07-31', NULL, 2, 0, 0);
INSERT INTO pieces VALUES('C02', 'Cadre', '87740679', 'C02', '17', '2000-07-31', NULL, 2, 0, 0);
INSERT INTO pieces VALUES('C87', 'Cadre', '87740679', 'C8', '23', '2000-07-31', NULL, 2, 9, 0);
INSERT INTO pieces VALUES('C87f', 'Cadre', '87740679', 'C8f', '25', '2000-07-31', NULL, 2, 15, 0);
INSERT INTO pieces VALUES('C25', 'Cadre', '69179980', 'C05', '19', '2000-07-31', NULL, 2, 9, 0);
INSERT INTO pieces VALUES('C26', 'Cadre', '69179980', 'S06', '19', '2000-07-31', NULL, 2, 6, 0);

INSERT INTO pieces VALUES('F9', 'Freins', '53116516', 'F9', '12', '2000-07-31', NULL, 2, 10, 0);

INSERT INTO pieces VALUES('G9', 'Guidon', '446634983', 'G2', '15', '2000-07-31', NULL, 2, 6, 0);
INSERT INTO pieces VALUES('G12', 'Guidon', '446634983', 'G3', '16', '2000-07-31', NULL, 2, 5, 0);

INSERT INTO pieces VALUES('S88', 'Selle', '08575264', 'S5', '18', '2000-07-31', NULL, 2, 5, 0);
INSERT INTO pieces VALUES('S37', 'Selle', '08575264', 'S1', '14', '2000-07-31', NULL, 2, 2, 0);
INSERT INTO pieces VALUES('S36', 'Selle', '08575264', 'S3', '14', '2000-07-31', NULL, 2, 2, 0);
INSERT INTO pieces VALUES('S34', 'Selle', '08575264', 'S2', '14', '2000-07-31', NULL, 2, 2, 0);
INSERT INTO pieces VALUES('S87', 'Selle', '08575264', 'S4', '18', '2000-07-31', NULL, 2, 5, 0);
INSERT INTO pieces VALUES('S02', 'Selle', '53116516', 'S8', '14', '2000-07-31', NULL, 2, 8, 0);
INSERT INTO pieces VALUES('S03', 'Selle', '53116516', 'S8', '14', '2000-07-31', NULL, 2, 1, 0);

INSERT INTO pieces VALUES('O2', 'Ordinateur', '87402867', 'O2', '32', '2000-07-31', NULL, 2, 9, 0);
INSERT INTO pieces VALUES('O4', 'Ordinateur', '87402867', 'O4', '34', '2000-07-31', NULL, 2, 7, 0);

INSERT INTO pieces VALUES('R02', 'Reflecteur', '72017119', 'R1', '7', '2000-07-31', NULL, 2, 10, 0);
INSERT INTO pieces VALUES('R09', 'Reflecteur', '72017119', 'R2', '7', '2000-07-31', NULL, 2, 10, 0);
INSERT INTO pieces VALUES('R10', 'Reflecteur', '72017119', 'R3', '7', '2000-07-31', NULL, 2, 10, 0);
INSERT INTO pieces VALUES('R45', 'Reflecteur', '72017119', 'R6', '7', '2000-07-31', NULL, 2, 7, 0);
INSERT INTO pieces VALUES('R46', 'Reflecteur', '72017119', 'R7', '7', '2000-07-31', NULL, 2, 7, 0);
INSERT INTO pieces VALUES('P1', 'Pedalier', '72017119', 'P1', '17', '2000-07-31', NULL, 2, 1, 0);
INSERT INTO pieces VALUES('P15', 'Pedalier', '72017119', 'P2', '15', '2000-07-31', NULL, 2, 0, 0);
INSERT INTO pieces VALUES('P34', 'Pedalier', '72017119', 'P3', '15', '2000-07-31', NULL, 2, 6, 0);

INSERT INTO pieces VALUES('S01', 'Panier', '72017119', 'S001', '8', '2000-07-31', NULL, 2, 8, 0);
INSERT INTO pieces VALUES('S05', 'Panier', '72017119', 'S005', '8', '2000-07-31', NULL, 2, 0, 0);
INSERT INTO pieces VALUES('S74', 'Panier', '72017119', 'S0074', '8', '2000-07-31', NULL, 2, 13, 0);
INSERT INTO pieces VALUES('S73', 'Panier', '72017119', 'S0073', '8', '2000-07-31', NULL, 2, 11, 0);

INSERT INTO pieces VALUES('DV133', 'Dérailleur Avant', '69179980', 'DV1', '16', '2000-07-31', NULL, 2, 9, 0);
INSERT INTO pieces VALUES('DV17', 'Dérailleur Avant', '69179980', 'DV2', '18', '2000-07-31', NULL, 2, 10, 0);
INSERT INTO pieces VALUES('DV87', 'Dérailleur Avant', '69179980', 'DV3', '18', '2000-07-31', NULL, 2, 15, 0);
INSERT INTO pieces VALUES('DV57', 'Dérailleur Avant', '66800362', 'DV001', '15', '2000-07-31', NULL, 2, 15, 0);
INSERT INTO pieces VALUES('DV15', 'Dérailleur Avant', '66800362', 'DV002', '16', '2000-07-31', NULL, 2, 11, 0);
INSERT INTO pieces VALUES('DV41', 'Dérailleur Avant', '72017119', 'DV0', '18', '2000-07-31', NULL, 2, 3, 0);
INSERT INTO pieces VALUES('DV132', 'Dérailleur Avant', '53116516', 'DV11', '17', '2000-07-31', NULL, 2, 1, 0);
INSERT INTO pieces VALUES('DV133', 'Dérailleur Avant', '53116516', 'DV12', '15', '2000-07-31', NULL, 2, 6, 0);

INSERT INTO pieces VALUES('DR56', 'Dérailleur Arrière', '69179980', 'DV1', '21', '2000-07-31', NULL, 2, 6, 0);
INSERT INTO pieces VALUES('DR87', 'Dérailleur Arrière', '69179980', 'DV2', '20', '2000-07-31', NULL, 2, 6, 0);
INSERT INTO pieces VALUES('DR86', 'Dérailleur Arrière', '69179980', 'DV3', '18', '2000-07-31', NULL, 2, 6, 0);
INSERT INTO pieces VALUES('DR23', 'Dérailleur Arrière', '446634983', 'DR1', '18', '2000-07-31', NULL, 2, 6, 0);
INSERT INTO pieces VALUES('DR76', 'Dérailleur Arrière', '53116516', 'DV01', '18', '2000-07-31', NULL, 2, 6, 0);
INSERT INTO pieces VALUES('DR52', 'Dérailleur Arrière', '53116516', 'DV02', '20', '2000-07-31', NULL, 2, 6, 0);


-- les clients individus 
INSERT INTO Clients VALUES (19656, 'BOIVIN', 'Louise', null, '1 rue de toulouse Paris 75015 ile de france', 'optic2000@hotmail.com',0618795620,null, false);
INSERT INTO Clients VALUES (19802, 'DION', 'Paul', null, '12 rue de versailles Cachan 94123 ile de france', 'bgyrt@hotmail.com', 0620135498, null,true);
INSERT INTO Clients VALUES (19954, 'BOUCHARD', 'Felix', null,'58 rue de boulot Alfort 91183 ile de france', 'koko@hotmail.com', 0618460797,null,true);
INSERT INTO Clients VALUES (19013, 'GENEST', 'Christian', null,'12 Rue Albert Camus Cachan 94123 ile de france', 'yyf@hotmail.com', 0672891403,null,true);
INSERT INTO Clients VALUES (16002, 'GAUTHIER', 'Fabrice', null,'12 Rue Anatole France Cachan 94123 ile de france', 'centaure@hotmail.com', 0794621621,null,true);
INSERT INTO Clients VALUES (19956, 'TREMBLAY', 'Jacques', null, '1 Rue Camille Desmoulins Paris 75015 ile de france', 'opticvie@hotmail.com', 0789461320,null,false);
INSERT INTO Clients VALUES (16561, 'SOUCY', 'Sybelle', null, '18 Rue de Flandre Cachan 94123 ile de france', 'qlf@hotmail.com', 0601985596,null,true);
INSERT INTO Clients VALUES (19925, 'DRECJACKSEN', 'Jack', null,'16 Rue de la Belle Image Cachan 94123 ile de france', 'pnl@hotmail.com',0799646420,null,true);
INSERT INTO Clients VALUES (19113,'SMITH', 'Marie', null,'10 Rue de la Marne Cachan 94123 ile de france', 'what@hotmail.com', 0651616289,null,true);
INSERT INTO Clients VALUES (19109,  'Delon', 'Alain', null,'1bis Rue de Lorraine Cachan 94123 ile de france', 'retgo@hotmail.com', 0618795630,null,true);
INSERT INTO Clients VALUES (19196, 'Auteuil', 'Daniel', null, '6 Rue des Lilas Paris 75015 ile de france', 'ghettho@hotmail.com', 0618797640,null,false);
INSERT INTO Clients VALUES (19182, 'Gabin', 'Jean', null, '97 Rue des Tournelles Cachan 94123 ile de france', 'bgdu95@hotmail.com', 0618785828,null,true);
INSERT INTO Clients VALUES (19864, 'Baumann', 'Lewis', null,'64 Rue du Docteur Gosselin Cachan 94123 ile de france', 'aptdf@hotmail.com', 0728795620,null,true);
INSERT INTO Clients VALUES (10000, 'Ahmed', 'Achirafi', null,'49 rue Mirabeau Cachan 94123 ile de france', 'ppt@hotmail.com', 0738392620,null,true);
INSERT INTO Clients VALUES (19999, 'Ait lahcen', 'Souhail', null,'49 rue Mirabeau Cachan 94123 ile de france', 'diapo@hotmail.com', 0710705620,null,true);

-- les clients entreprises (commence par 2 le id)
INSERT INTO Clients VALUES (29125, null, null, 'Net','12 Rue Champs-Elysée 75001 Paris', 'Net@hotmail.com',0610705020,'Abdel Koichi', true);
INSERT INTO Clients VALUES (29713, null, null,'PORT','1 Rue du Rungis Cachan 94123 ile de france', 'port@hotmail.com', 0134321520,'Nathan Dupond',true);
INSERT INTO Clients VALUES (29675, null, null, 'MKL','1 Rue de la Grange Ory Cachan 94123 ile de france', 'mkl@hotmail.com', 0118795620,'Pierre Louis',true);
INSERT INTO Clients VALUES (29607, null, null, 'Pokemon', '3498 Rue des Amandiers Paris 75015 ile de france', 'pokemon@hotmail.com', 0918731639,'Etienne Legrange',false);
INSERT INTO Clients VALUES (20564, null, null, 'Nintendio', '18 Rue des Vignes Cachan 94123 ile de france', 'nintendio@hotmail.com', 0117597620,'Alpha Beta',true);
INSERT INTO Clients VALUES (21368,null, null, 'Twittor','1bis rue de versaille Cachan 94123 ile de france', 'twittor@hotmail.com', 0118791610,'Sun Shine',true);

INSERT INTO PieceFournisseur VALUES(446634983, "C1");
INSERT INTO PieceFournisseur VALUES(446634983, "C2");
INSERT INTO PieceFournisseur VALUES(446634983, "C3");
INSERT INTO PieceFournisseur VALUES(446634983, "C4");
INSERT INTO PieceFournisseur VALUES(446634983, "C5");
INSERT INTO PieceFournisseur VALUES(446634983, "C6");
INSERT INTO PieceFournisseur VALUES(446634983, "G1");
INSERT INTO PieceFournisseur VALUES(446634983, "G2");
INSERT INTO PieceFournisseur VALUES(446634983, "G3");
INSERT INTO PieceFournisseur VALUES(446634983, "G4");
INSERT INTO PieceFournisseur VALUES(446634983, "G5");
INSERT INTO PieceFournisseur VALUES(446634983, "G6");

INSERT INTO PieceFournisseur VALUES(53116516, "C1");
INSERT INTO PieceFournisseur VALUES(53116516, "C2");
INSERT INTO PieceFournisseur VALUES(53116516, "C3");
INSERT INTO PieceFournisseur VALUES(53116516, "C4");
INSERT INTO PieceFournisseur VALUES(53116516, "C5");
INSERT INTO PieceFournisseur VALUES(53116516, "C6");
INSERT INTO PieceFournisseur VALUES(53116516, "F1");
INSERT INTO PieceFournisseur VALUES(53116516, "F2");
INSERT INTO PieceFournisseur VALUES(53116516, "F3");
INSERT INTO PieceFournisseur VALUES(53116516, "F4");
INSERT INTO PieceFournisseur VALUES(53116516, "F5");

INSERT INTO PieceFournisseur VALUES(48899036, "S1");
INSERT INTO PieceFournisseur VALUES(48899036, "S2");
INSERT INTO PieceFournisseur VALUES(48899036, "S3");
INSERT INTO PieceFournisseur VALUES(48899036, "S4");
INSERT INTO PieceFournisseur VALUES(48899036, "S5");
INSERT INTO PieceFournisseur VALUES(48899036, "S6");
INSERT INTO PieceFournisseur VALUES(48899036, "S7");
INSERT INTO PieceFournisseur VALUES(48899036, "S8");

INSERT INTO PieceFournisseur VALUES(69179980, "R1");
INSERT INTO PieceFournisseur VALUES(69179980, "R2");
INSERT INTO PieceFournisseur VALUES(69179980, "R3");
INSERT INTO PieceFournisseur VALUES(69179980, "R4");
INSERT INTO PieceFournisseur VALUES(69179980, "R5");
INSERT INTO PieceFournisseur VALUES(69179980, "R6");
INSERT INTO PieceFournisseur VALUES(69179980, "R7");
INSERT INTO PieceFournisseur VALUES(69179980, "R8");

INSERT INTO PieceFournisseur VALUES(72017119, "DV1");
INSERT INTO PieceFournisseur VALUES(72017119, "DV2");
INSERT INTO PieceFournisseur VALUES(72017119, "DV3");
INSERT INTO PieceFournisseur VALUES(72017119, "DV4");
INSERT INTO PieceFournisseur VALUES(72017119, "DV5");
INSERT INTO PieceFournisseur VALUES(72017119, "DV6");
INSERT INTO PieceFournisseur VALUES(72017119, "DV7");
INSERT INTO PieceFournisseur VALUES(72017119, "DV8");

INSERT INTO PieceFournisseur VALUES(70157979, "P1");
INSERT INTO PieceFournisseur VALUES(70157979, "P2");
INSERT INTO PieceFournisseur VALUES(70157979, "P3");
INSERT INTO PieceFournisseur VALUES(70157979, "P4");
INSERT INTO PieceFournisseur VALUES(70157979, "P5");
INSERT INTO PieceFournisseur VALUES(70157979, "P6");
INSERT INTO PieceFournisseur VALUES(70157979, "P7");
INSERT INTO PieceFournisseur VALUES(70157979, "P8");
INSERT INTO PieceFournisseur VALUES(70157979, "P9");


insert into ASSEMBLAGE values ('101','C32','G7','F3','S88','DV133','DR56','R45','R46',NULL,'P12','O2',NULL);
insert into ASSEMBLAGE values ('102','C34','G7','F3','S88','DV17','DR87','R48','R47',NULL,'P12',NULL,NULL);
insert into ASSEMBLAGE values ('103','C76','G7','F3','S88','DV17','DR87','R48','R47',NULL,'P12','O2',NULL);
insert into ASSEMBLAGE values ('104','C76','G7','F3','S88','DV87','DR86','R12','R32',NULL,'P12',NULL,NULL);
insert into ASSEMBLAGE values ('105','C43','G9','F9','S37','DV57','DR86','R19','R18','R02','P34',NULL,NULL);
insert into ASSEMBLAGE values ('106','C44f','G9','F9','S35','DV57','DR86','R19','R18','R02','P34',NULL,NULL);
insert into ASSEMBLAGE values ('107','C43','G9','F9','S37','DV57','DR87','R19','R18','R02','P34','O4',NULL);
insert into ASSEMBLAGE values ('108','C43f','G9','F9','S35','DV57','DR87','R19','R18','R02','P34','O4',NULL);
INSERT INTO ASSEMBLAGE VALUES(109, 'C01', 'G12', NULL, 'S02', NULL, NULL, 'R1', 'R2', 'R09', 'P1', NULL, 'S01');
INSERT INTO ASSEMBLAGE VALUES(110, 'C02', 'G12', NULL, 'S03', NULL, NULL, 'R1', 'R2', 'R09', 'P1', NULL, 'S05');
INSERT INTO ASSEMBLAGE VALUES(111, 'C15', 'G12', 'F9', 'S36', 'DV15', 'DR23', 'R11', 'R12', 'R10', 'P15', NULL, 'S74');
INSERT INTO ASSEMBLAGE VALUES(112, 'C87', 'G12', 'F9', 'S36', 'DV41', 'DR76', 'R11', 'R12', 'R10', 'P15', NULL, 'S74');
INSERT INTO ASSEMBLAGE VALUES(113, 'C87f', 'G12', 'F9', 'S34', 'DV41', 'DR76', 'R11', 'R12', 'R10', 'P15', NULL, 'S73');
INSERT INTO ASSEMBLAGE VALUES(114, 'C25', 'G7', 'F3', 'S87', 'DV132', 'DR52', 'R44', 'R47', NULL, 'P12', NULL, NULL);
INSERT INTO ASSEMBLAGE VALUES(115, 'C26', 'G7', 'F3', 'S87', 'DV133', 'DR52', 'R44', 'R47', NULL, 'P12', NULL, NULL);


INSERT  INTO  COMMANDE  VALUES ('CO10254',  19656 , '2021-04-27', '5 rue des champs','2021-05-01');
INSERT  INTO  COMMANDE  VALUES ('CO10255', 19864, '2021-04-27', '1 rue de la république','2021-05-04');
INSERT  INTO  COMMANDE  VALUES ('CO10256', 16002, '2021-04-28', '24 quartier du méridien','2021-05-07');
INSERT  INTO  COMMANDE  VALUES ('CO10257', 19182, '2021-04-30', '52 avenue François Miterrand','2021-05-07');
INSERT  INTO  COMMANDE  VALUES ('CO10258', 19113, '2021-04-30', '3 rue des glaces','2021-05-08');
INSERT  INTO  COMMANDE  VALUES ('CO10259', 19656, '2021-05-01', '15 rue Charles de Gaulle','2021-05-08');
INSERT  INTO  COMMANDE  VALUES ('CO10260', 10000, '2021-05-02', '5 rue des champs','2021-05-14');
INSERT  INTO  COMMANDE  VALUES ('CO10261',19999, '2021-05-02', '5 rue des champs','2021-05-13');

INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10261','P12', 3);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10254', '101', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10255', '111', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10255', '102', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10256', '114', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10257', '102', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10258', '106', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10259', '107', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10260', 'F3', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10260', 'S88', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10260', 'R47', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10260', 'R48', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10260','DV17', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10261','F3', 1);
INSERT  INTO  ITEMCOMMANDE  VALUES ('CO10261','S87', 1);

-- insertion caractéristique de la carte fidélité
INSERT INTO Car_Fidelio VALUES (1, 'Fidelio', 15, '1 an', 5);
INSERT INTO Car_Fidelio VALUES (2, 'Fidelio Or', 25, '2 ans', 8);
INSERT INTO Car_Fidelio VALUES (3, 'Fidelio Platine', 60, '2 ans', 10);
INSERT INTO Car_Fidelio VALUES (4, 'Fidelio Max', 100, '3 ans', 12);

-- insertion carte fidélité
INSERT INTO Fidelio VALUES (19656, 1, '2020-05-30');
INSERT INTO Fidelio VALUES (19802, 1, '2020-12-02');
INSERT INTO Fidelio VALUES (19954, 2, '2020-11-12');
INSERT INTO Fidelio VALUES (19013, 3, '2020-05-08');
INSERT INTO Fidelio VALUES (19196, 4, '2019-04-18');
INSERT INTO Fidelio VALUES (10000, 3, '2017-03-29');
INSERT INTO Fidelio VALUES (19999, 4, '2017-01-10');
INSERT INTO Fidelio VALUES (19925, 1, '2016-05-09');