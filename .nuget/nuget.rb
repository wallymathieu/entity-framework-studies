module NuGet
  def self.os
    @os ||= (
      host_os = RbConfig::CONFIG['host_os']
      case host_os
      when /mswin|msys|mingw|cygwin|bccwin|wince|emc/
        :windows
      when /darwin|mac os/
        :macosx
      when /linux/
        :linux
      when /solaris|bsd/
        :unix
      else
        raise Error::WebDriverError, "unknown os: #{host_os.inspect}"
      end
    )
  end

  def self.exec(parameters)

    command = File.join(File.dirname(__FILE__),"NuGet.exe")
    if self.os == :windows
      system "#{command} #{parameters}"
    else
      system "mono --runtime=v4.0.30319 #{command} #{parameters} "
    end
  end

  def self.raise_if_not_any?(cmds, expected)
    if !cmds.any?
      raise "Could not find #{expected}!"
    end
  end

  def self.nunit_path
    cmds = Dir.glob(File.join(File.dirname(__FILE__),"..","packages","NUnit.Runners.*","tools","nunit-console.exe"))
    self.raise_if_not_any?(cmds, "nunit runner")
    return cmds.first
  end

  def self.nunit_86_path
    cmds = Dir.glob(File.join(File.dirname(__FILE__),"..","packages","NUnit.Runners.*","tools","nunit-console-x86.exe"))
    self.raise_if_not_any?(cmds, "nunit runner")
    return cmds.first
  end

  def self.xunit_path
    cmds = Dir.glob(File.join(File.dirname(__FILE__),"..","packages","xunit.runners.*","tools","xunit.console.clr4.exe"))
    self.raise_if_not_any?(cmds, "xunit runner")
    return cmds.first
  end

  def self.migrate_path
    cmds = Dir.glob(File.join(File.dirname(__FILE__),"..","packages","FluentMigrator.*","tools","Migrate.exe"))
    self.raise_if_not_any?(cmds, "migrate runner")
    return cmds.first
  end

end
