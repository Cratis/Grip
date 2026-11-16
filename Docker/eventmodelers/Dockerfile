# A Studio builder for projects built with the Eventmodelers BuildKit: the Cratis builder image with Node.js and the adapter.
# The kit itself is not in the image; it lives in the project (`npx @eventmodelers/cli init`). Published to Docker Hub as
# cratis/studio-eventmodelers-builder with every Grip release, on the cratis/builder image of the same release. Build it
# from the repository root:
#
#   docker build -f Docker/eventmodelers/Dockerfile -t cratis/studio-eventmodelers-builder:latest .
ARG BUILDER_VERSION=latest
FROM cratis/builder:${BUILDER_VERSION}

COPY Docker/eventmodelers/eventmodelers-builder.mjs Docker/eventmodelers/slices.mjs Docker/eventmodelers/package.json /opt/eventmodelers/

CMD ["build", "run", "--project", "/workspace", "--exec", "node /opt/eventmodelers/eventmodelers-builder.mjs"]
