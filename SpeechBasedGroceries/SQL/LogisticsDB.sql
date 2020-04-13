
-- PostgresSQL DB running on Azure
/* -- MOVED TO MSSQL-DB BECAUSE IT'S CHEAPER
CREATE TABLE t_delivery (
  f_del_id SERIAL NOT NULL,
  f_del_customerno INT NULL,
  f_del_date DATE NULL,
  f_del_street VARCHAR(255),
  f_del_zip VARCHAR(255),
  f_del_city VARCHAR(255),
  f_del_country VARCHAR(255),
  f_del_comment VARCHAR(255),
  PRIMARY KEY (f_del_id)
);

CREATE TABLE t_position (
  f_pos_id SERIAL NOT NULL,
  f_del_id INT NOT NULL,
  f_pos_no INT NULL,
  f_pos_itemid VARCHAR(255) NULL,
  f_pos_itemtext VARCHAR(255) NULL,
  f_pos_itemqty INT NULL,
  f_pos_itemprice DOUBLE PRECISION NULL,
  f_pos_itemweight DOUBLE PRECISION NULL,
  f_pos_comment VARCHAR(255) NULL,
  PRIMARY KEY (f_pos_id),
  FOREIGN KEY(f_del_id)
    REFERENCES t_delivery(f_del_id)
);
*/ 

DROP TABLE [dbo].[t_position]
DROP TABLE [dbo].[t_delivery]

CREATE TABLE [dbo].[t_delivery] (
  f_del_id INT IDENTITY(100,1) PRIMARY KEY,
  f_del_customerid INT NOT NULL,
  f_del_date DATE NULL,
  f_del_street VARCHAR(255),
  f_del_zip VARCHAR(255),
  f_del_city VARCHAR(255),
  f_del_country VARCHAR(255),
  f_del_comment VARCHAR(255),
);

CREATE TABLE [dbo].[t_position] (
  f_pos_id INT IDENTITY(2000,1) PRIMARY KEY,
  f_del_id INT NOT NULL FOREIGN KEY REFERENCES [dbo].[t_delivery](f_del_id),
  f_pos_no INT NULL,
  f_pos_itemid VARCHAR(255) NULL,
  f_pos_itemtext VARCHAR(255) NULL, 
  f_pos_itemqty VARCHAR(255) NULL, 
  f_pos_itemprice VARCHAR(255) NULL, 
  f_pos_itemweight VARCHAR(255) NULL, 
  f_pos_comment VARCHAR(255) NULL
);


INSERT INTO [dbo].[t_delivery] (f_del_customerid, f_del_date, f_del_street, f_del_zip, f_del_city, f_del_country, f_del_comment) VALUES
(1,	'2020-03-22',	'Am Riemen 33',	'5000',	'Aarau',	'Schweiz',	'Tel. 079 123 45 67'),
(1,	'2020-04-09',	'Bellevue 9a',	'9000',	'St. Gallen',	'Schweiz',	null),
(3,	'2020-04-19',	'Bellevue 9a',	'9000',	'St. Gallen',	'Schweiz',	'Kann beim Nachbarn hinterlegt werden');

INSERT INTO [dbo].[t_position] (f_del_id, f_pos_no, f_pos_itemid, f_pos_itemtext, f_pos_itemqty, f_pos_itemprice, f_pos_itemweight, f_pos_comment) VALUES
((SELECT f_del_id FROM t_delivery WHERE f_del_date='2020-03-22'), 10, 7610029188738, 'Kaffeekapseln Vivente', 1, 9.95, 0.15, null),
((SELECT f_del_id FROM t_delivery WHERE f_del_date='2020-04-09'), 10, 3700067800045, 'Chardonnay', 6, 23.45, 1.5, null),
((SELECT f_del_id FROM t_delivery WHERE f_del_date='2020-04-09'), 20, 5010677925006, 'Martini Bianco', 3, 29.95, 1.9, null),
((SELECT f_del_id FROM t_delivery WHERE f_del_date='2020-04-09'), 30, 7610200428059, 'Bratwurst', 20, 4.95, 0.35, 'MBudget'),
((SELECT f_del_id FROM t_delivery WHERE f_del_date='2020-04-19'), 10, 7610029137903, 'Chäs-Chüechli Ramequins', 4, 10.10, 0.275, 'Tortine al fromaggio');

