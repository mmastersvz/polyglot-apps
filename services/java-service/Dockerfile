
FROM eclipse-temurin:21-jre-alpine

ENV PORT=8080
ENV CLASSPATH=/opt/lib
EXPOSE 8080

RUN addgroup -g 1001 -S appgroup && \
    adduser -u 1001 -S -G appgroup appuser

# copy pom.xml and wildcards to avoid this command failing if there's no target/lib directory
COPY pom.xml target/lib* /opt/lib/

COPY target/*.jar /opt/app.jar

COPY docker-entrypoint.sh /opt/docker-entrypoint.sh
RUN chmod +x /opt/docker-entrypoint.sh

# Switch to non-root user
USER 1001:1001

WORKDIR /opt
ENTRYPOINT ["/opt/docker-entrypoint.sh"]