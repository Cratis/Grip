# The packaged Axon builder: the Cratis builder image with the Axon kit, so `cratis build run --level code --target axon`
# renders the model into a Java application on Axon Framework and verifies it with Maven. Published to Docker Hub as
# cratis/studio-axon-builder with every Grip release, on the cratis/builder image of the same release. Build it from the
# repository root:
#
#   docker build -f Docker/axon/Dockerfile -t cratis/studio-axon-builder:latest .
#
# Run it with the CLI, which picks this image for the axon target: cratis build run --level code --target axon --docker
ARG BUILDER_VERSION=latest
FROM cratis/builder:${BUILDER_VERSION}

COPY Kits/axon /opt/cratis/kits/axon

# The same command the CLI runs: the kit tells the builder how to verify and what it may write.
CMD ["build", "run", "--project", "/workspace", "--level", "code", "--target", "axon", "--kit", "/opt/cratis/kits/axon"]
