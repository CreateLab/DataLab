db.createUser(
    {
        user: "admin",
        pwd: "admin",
        roles: [
            {
                role: "readWrite",
                db: "st_and_db_mongo_database"
            }
        ]
    }
);
