
-- MsSQL DB running on Azure

CREATE TABLE [dbo].[t_customer] (
  f_cus_id INT IDENTITY(1,1) PRIMARY KEY,
  f_cus_no INT NULL,
  f_cus_firstname VARCHAR(255),
  f_cus_surname VARCHAR(255),
  f_cus_birthdate DATE NULL,
  f_cus_street VARCHAR(255),
  f_cus_zip VARCHAR(255),
  f_cus_city VARCHAR(255),
  f_cus_country VARCHAR(255),
  f_cus_email VARCHAR(255)
);


CREATE TABLE [dbo].[t_token] (
  f_tok_id INT IDENTITY(1,1) PRIMARY KEY,
  f_cus_id INT NOT NULL FOREIGN KEY REFERENCES t_customer(f_cus_id),
  f_tok_creation DATE NULL,
  f_tok_name VARCHAR(255),
  f_tok_value VARCHAR(255),
  f_tok_expiration DATE NULL
);


INSERT INTO t_customer (f_cus_no, f_cus_firstname, f_cus_surname, f_cus_birthdate, f_cus_street, f_cus_zip, f_cus_city, f_cus_country, f_cus_email) VALUES
(2501,	'Lukas',	'Neubrauner',	'1994-11-24',	'Am Riemen 33',	'5000',	'Aarau',	'Schweiz',	'lukas.neub@eai.com'),
(2502,	'Anna',	'Puiz',	'1982-06-13',	'Albisweg 2',	'8048',	'Zurich',	'Schweiz',	'annapuiz@eai.com'),
(2503,	'Maximilian',	'Grob',	'2000-07-03',	'Bellevue 9a',	'9000',	'St. Gallen',	'Schweiz',	'mgrob@eai.com')
