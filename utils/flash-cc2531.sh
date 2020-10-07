#!/bin/bash
set -e

function cleanUp() {
  rm -rf ./flash_cc2531
  rm -rf ./firmware
  rm -rf master.zip
  rm -rf ./bin
}

cleanUp

if ! command -v gpio &> /dev/null
then
    echo "You must install wiringpi. Run sudo apt-get install wiringpi"
    exit
fi

git clone --depth 1 https://github.com/jmichault/flash_cc2531.git


chipId=`./flash_cc2531/cc_chipid`
if [ -z "$(echo $chipId | grep 'b524')" ];
then
echo "wrong chip: $chipId. Check your wiring and run script again.";
exit 1
fi;


rm -rf ./firmware

mkdir firmware
wget https://github.com/Koenkk/Z-Stack-firmware/archive/master.zip
unzip master.zip -d ./firmware
rm master.zip

mkdir bin
unzip ./firmware/Z-Stack-firmware-master/coordinator/Z-Stack_Home_1.2/bin/default/CC2531_DEFAULT_20190608.zip -d ./bin


read -p $'Erase CC2531 dongle? [y/n]\n' confirm

if [ "$confirm" != "y" ];
then
echo "Exiting..."
exit 2
fi

./flash_cc2531/cc_erase

read -p $'Flash CC2531 dongle? [y/n]\n' confirm

if [ "$confirm" != "y" ];
then
echo "Exiting..."
exit 3
fi


./flash_cc2531/cc_write ./bin/CC2531ZNP-Prod.hex

cleanUp

echo "Done!"
