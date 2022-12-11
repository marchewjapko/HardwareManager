FROM ubuntu:22.04
RUN apt-get update -y
RUN apt-get install -y libicu-dev
RUN apt install -y tcpdump
RUN apt-get install -y sysstat
RUN apt-get install -y ethtool
RUN apt-get install -y wireless-tools
RUN apt-get install -y ifstat
COPY CounterTester /
EXPOSE 80
ENTRYPOINT "/CounterTester"