FROM alpine:3.19

# Install certificates for secure outgoing calls (APIs, etc.)
RUN apk --no-cache add ca-certificates

# Create a non-root user and group
RUN addgroup -g 1001 -S appgroup && \
    adduser -u 1001 -S -G appgroup appuser

WORKDIR /app

COPY app .

# Fix permissions so the non-root user can read/execute it
RUN chown -R appuser:appgroup /app && \
    chmod +x /app/app

# Switch to non-root user
USER 1001:1001

# Ensure the binary has execution permissions
RUN chmod +x app

USER 1001:1001

EXPOSE 8080

CMD ["./app"]