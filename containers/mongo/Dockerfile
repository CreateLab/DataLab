FROM mongo
ENV MONGO_INITDB_ROOT_USERNAME admin
ENV MONGO_INITDB_ROOT_PASSWORD admin
ENV MONGO_INITDB_DATABASE st_and_db_mongo_database
COPY ./init.js /docker-entrypoint-initdb.d/
