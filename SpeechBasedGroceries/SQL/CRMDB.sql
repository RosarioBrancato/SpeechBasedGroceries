
-- MsSQL DB running on Azure

DROP TABLE [dbo].[t_token]
DROP TABLE [dbo].[t_customer]

CREATE TABLE [dbo].[t_customer] (
  f_cus_id INT IDENTITY(1,1) PRIMARY KEY,
  f_cus_firstname VARCHAR(255),
  f_cus_surname VARCHAR(255),
  f_cus_birthdate DATE NULL,
  f_cus_street VARCHAR(255),
  f_cus_zip VARCHAR(255),
  f_cus_city VARCHAR(255),
  f_cus_country VARCHAR(255),
  f_cus_email VARCHAR(255),
  f_cus_telegramid INT NULL
);


CREATE TABLE [dbo].[t_token] (
  f_tok_id INT IDENTITY(1,1) PRIMARY KEY,
  f_cus_id INT NOT NULL FOREIGN KEY REFERENCES t_customer(f_cus_id),
  f_tok_creation DATE NULL,
  f_tok_name VARCHAR(255),
  f_tok_value VARCHAR(600),
  f_tok_expiration DATE NULL
);


INSERT INTO [dbo].[t_customer] (f_cus_firstname, f_cus_surname, f_cus_birthdate, f_cus_street, f_cus_zip, f_cus_city, f_cus_country, f_cus_email, f_cus_telegramid) VALUES
('Tobias',	'Neubrauner',	null,	'Am Riemen 33',	'5000',	'Aarau',	'Schweiz',	'neubrauner@eai.com', 1000000),
('Rosario',	'Puiz',	'1982-06-13',	'Albisweg 2',	'8048',	'Zurich',	'Schweiz',	null, 1000001),
('Roman',	'Grob',	'2000-07-03',	'Bellevue 9a',	'9000',	'St. Gallen',	'Schweiz',	'grob@eai.com', 1000001);

INSERT INTO [dbo].[t_token] (f_cus_id, f_tok_creation, f_tok_name, f_tok_value, f_tok_expiration) VALUES
((SELECT f_cus_id FROM t_customer WHERE f_cus_telegramid=1000000), null, 'Fridgy', 'abcde', null),
((SELECT f_cus_id FROM t_customer WHERE f_cus_telegramid=1000001), null, 'Fridgy', 'fghij', null),
((SELECT f_cus_id FROM t_customer WHERE f_cus_telegramid=1000001), null, 'Fridgy', 'klmno', null);
