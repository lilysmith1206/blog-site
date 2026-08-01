FROM ghcr.io/lilysmith1206/aspnet-with-curl:latest AS base

ENV HEALTH_CHECK_PATH=/health
ENV HEALTH_CHECK_PORT=8080

HEALTHCHECK --interval=10s --timeout=3s --start-period=5s --retries=3 \
    CMD curl --fail "http://localhost:${HEALTH_CHECK_PORT}${HEALTH_CHECK_PATH}" || exit 1