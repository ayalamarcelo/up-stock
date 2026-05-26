CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE status (
    statusID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    nameStatus VARCHAR(150) NOT NULL UNIQUE,
    description VARCHAR(255)
);

CREATE TABLE category (
    categoryID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    nameCategory VARCHAR(150) NOT NULL UNIQUE
);

CREATE TABLE users (
    userID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(150) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    rol VARCHAR(50) NOT NULL,
    isActive BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE clients (
    clientID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(150) NOT NULL,
    dniCuit VARCHAR(50) NOT NULL UNIQUE,
    phone VARCHAR(50),
    isActive BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE assets (
    assetID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    categoryID UUID NOT NULL,
    statusID UUID NOT NULL,
    name VARCHAR(150) NOT NULL,
    codeID VARCHAR(100) NOT NULL UNIQUE,
    isDeleted BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT fk_assets_category FOREIGN KEY (categoryID) REFERENCES category(categoryID) ON DELETE RESTRICT,
    CONSTRAINT fk_assets_status FOREIGN KEY (statusID) REFERENCES status(statusID) ON DELETE RESTRICT
);

CREATE TABLE rental (
    rentalID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    statusID UUID NOT NULL,
    clientID UUID NOT NULL,
    userID UUID NOT NULL,
    rentalDate TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    rentalDateExpected TIMESTAMPTZ NOT NULL,
    CONSTRAINT fk_rental_status FOREIGN KEY (statusID) REFERENCES status(statusID) ON DELETE RESTRICT,
    CONSTRAINT fk_rental_clients FOREIGN KEY (clientID) REFERENCES clients(clientID) ON DELETE RESTRICT,
    CONSTRAINT fk_rental_users FOREIGN KEY (userID) REFERENCES users(userID) ON DELETE RESTRICT
);

CREATE TABLE rentalItems (
    rentalItemsID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    rentalID UUID NOT NULL,
    asset_id UUID NOT NULL,
    returnDateActual TIMESTAMPTZ,
    conditionOnReturn TEXT,
    CONSTRAINT fk_rentalitems_rental FOREIGN KEY (rentalID) REFERENCES rental(rentalID) ON DELETE CASCADE,
    CONSTRAINT fk_rentalitems_assets FOREIGN KEY (asset_id) REFERENCES assets(assetID) ON DELETE RESTRICT
);

CREATE TABLE maintenanceLogs (
    maintenanceID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    categoryID UUID NOT NULL,
    statusID UUID NOT NULL,
    assetID UUID NOT NULL,
    maintenanceDate TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    observations VARCHAR(255),
    CONSTRAINT fk_maintenance_category FOREIGN KEY (categoryID) REFERENCES category(categoryID) ON DELETE RESTRICT,
    CONSTRAINT fk_maintenance_status FOREIGN KEY (statusID) REFERENCES status(statusID) ON DELETE RESTRICT,
    CONSTRAINT fk_maintenance_assets FOREIGN KEY (assetID) REFERENCES assets(assetID) ON DELETE RESTRICT
);

CREATE TABLE assetLogs (
    logID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    assetID UUID NOT NULL,
    statusID UUID NOT NULL,
    userID UUID NOT NULL,
    logDate TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    observations VARCHAR(255),
    CONSTRAINT fk_logs_assets FOREIGN KEY (assetID) REFERENCES assets(assetID) ON DELETE CASCADE,
    CONSTRAINT fk_logs_status FOREIGN KEY (statusID) REFERENCES status(statusID) ON DELETE RESTRICT,
    CONSTRAINT fk_logs_users FOREIGN KEY (userID) REFERENCES users(userID) ON DELETE RESTRICT
);